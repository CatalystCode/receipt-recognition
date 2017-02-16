# -*- coding: utf-8 -*-
"""
Created on Wed Nov  2 10:28:18 2016
@author: chyam
purpose: load images from blob storage, post to Microsoft OCR, save results, populate 'text' to .tsv file.
"""
### Error: UnicodeEncodeError: 'charmap' codec can't encode character
###     Fixes: at windows console, type chcp 65001, press enter

from __future__ import print_function
from azure.storage.blob import BlockBlobService
from azure.storage.blob import PublicAccess
import configparser
import time
import requests
import json
import pandas as pd
from io import StringIO

_url = 'https://api.projectoxford.ai/vision/v1.0/ocr'
_maxNumRetries = 10

def main():
    
    # Get credential
    parser = configparser.ConfigParser()
    parser.read('config.ini')
    STORAGE_ACCOUNT_NAME = parser.get('credential', 'STORAGE_ACCOUNT_NAME')    
    STORAGE_ACCOUNT_KEY = parser.get('credential', 'STORAGE_ACCOUNT_KEY')
    CONTAINER_NAME = parser.get('credential', 'CONTAINER_NAME')
    CONTAINER_NAME_METADATA = parser.get('credential', 'CONTAINER_NAME_METADATA')
    VISION_API_KEY = parser.get('credential', 'VISION_API_KEY') # need to use Agitare account
    CONTAINER_NAME_OCR_2 = parser.get('credential', 'CONTAINER_NAME_OCR_2')    
    CONTAINER_NAME_STRUCTUREDDATA = parser.get('credential', 'CONTAINER_NAME_STRUCTUREDDATA')

    # access to bloob
    block_blob_service = BlockBlobService(account_name=STORAGE_ACCOUNT_NAME, account_key=STORAGE_ACCOUNT_KEY)
    block_blob_service.set_container_acl(CONTAINER_NAME, public_access=PublicAccess.Container)
   
    # empty dataframe
    df = pd.DataFrame({'Text' : [], 'Category' : [], 'ReceiptID' : []})
    
    # get url and label from file
    blob_text = block_blob_service.get_blob_to_text(CONTAINER_NAME_METADATA, 'image-url-6-classes.tsv'); #print(blob_text.content)
    df_url_label = pd.DataFrame.from_csv(StringIO(blob_text.content), index_col=None, sep='\t'); #print(df_url_label.shape); print(df_url_label)
    
    for index, row in df_url_label.iterrows():
        #print(row['Category'])
        #print(row['Url'])
   
        imageurl = row['Url']
        # OCR parameters
        params = { 'language': 'en', 'detectOrientation ': 'true'} 
        headers = dict()
        headers['Ocp-Apim-Subscription-Key'] =  VISION_API_KEY
        headers['Content-Type'] = 'application/json' 
        
        image_url = { 'url': imageurl } ; 
        image_file = None
        result = processRequest( image_url, image_file, headers, params )
        
        if result is not None:
            #print(result)
            result_str = json.dumps(result); #print(result_str)
            
            # write result into blob
            ocrblobname = imageurl[-17:-3] + 'json'
            block_blob_service.create_blob_from_text(CONTAINER_NAME_OCR_2, ocrblobname, result_str)
            # extract text
            text = extractText(result); #print (text)
            
            # populate dataframe
            df.loc[index,'Text'] = text
        else:
            # populate dataframe
            df.loc[index,'Text'] = None
                        
        df.loc[index,'Category'] = row['Category']
        df.loc[index,'ReceiptID'] = imageurl[-17:-3]
            
    # write dataframe to blob
    print("-----------------------")
    df_str = df.to_csv(sep='\t', index=False); 
    
    dfblobname = 'dataframe_6_classes_url.tsv' 
    block_blob_service.create_blob_from_text(CONTAINER_NAME_STRUCTUREDDATA, dfblobname, df_str) 

    return
    
def extractText(result):
    text = ""
    for region in result['regions']:
        for line in region['lines']:
            for word in line['words']:
                #print (word.get('text'))
                text = text + " " + word.get('text')
    return text
    
def processRequest( image_url, image_file, headers, params ):

    """
    Ref: https://github.com/Microsoft/Cognitive-Vision-Python/blob/master/Jupyter%20Notebook/Computer%20Vision%20API%20Example.ipynb
    Helper function to process the request to Project Oxford
    Parameters:
    json: Used when processing images from its URL. See API Documentation
    data: Used when processing image read from disk. See API Documentation
    headers: Used to pass the key information and the data type request
    """

    retries = 0
    result = None

    while True:

        response = requests.request( 'post', _url, json = image_url, data = image_file, headers = headers, params = params )
        
        if response.status_code == 429: 

            print( "Message: %s" % ( response.json()['message'] ) )

            if retries <= _maxNumRetries: 
                time.sleep(1) 
                retries += 1
                continue
            else: 
                print( 'Error: failed after retrying!' )
                break

        elif response.status_code == 200 or response.status_code == 201:

            if 'content-length' in response.headers and int(response.headers['content-length']) == 0: 
                result = None 
            elif 'content-type' in response.headers and isinstance(response.headers['content-type'], str): 
                if 'application/json' in response.headers['content-type'].lower(): 
                    result = response.json() if response.content else None 
                elif 'image' in response.headers['content-type'].lower(): 
                    result = response.content
        else:
            print(response.json()) 
            print( "Error code: %d" % ( response.status_code ) ); 
            print( "Message: %s" % ( response.json()['message'] ) ); 

        break
        
    return result
    
if __name__ == '__main__':
    main() 