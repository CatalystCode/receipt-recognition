# -*- coding: utf-8 -*-
"""
Created on Tue Nov  8 21:08:16 2016

@author: chyam
"""

from __future__ import print_function
from azure.storage.blob import BlockBlobService
from azure.storage.blob import PublicAccess
import configparser
import pandas as pd
from io import StringIO

def main():
    
    # Get credential
    parser = configparser.ConfigParser()
    parser.read('config.ini')
    STORAGE_ACCOUNT_NAME = parser.get('credential', 'STORAGE_ACCOUNT_NAME')    
    STORAGE_ACCOUNT_KEY = parser.get('credential', 'STORAGE_ACCOUNT_KEY')
    CONTAINER_NAME = parser.get('credential', 'CONTAINER_NAME')
    CONTAINER_NAME_METADATA = parser.get('credential', 'CONTAINER_NAME_METADATA')

    # access to blob storage
    block_blob_service = BlockBlobService(account_name=STORAGE_ACCOUNT_NAME, account_key=STORAGE_ACCOUNT_KEY)
    block_blob_service.set_container_acl(CONTAINER_NAME, public_access=PublicAccess.Container)
    generator = block_blob_service.list_blobs(CONTAINER_NAME)
   
    # empty dataframe
    df = pd.DataFrame({'Url' : [], 'Category' : []})
    
    # get label from file
    blob_text = block_blob_service.get_blob_to_text(CONTAINER_NAME_METADATA, 'receipt_list_labelled.csv');  #print(blob_text.content)
    df_label = pd.DataFrame.from_csv(StringIO(blob_text.content), index_col=None, sep=','); #print(df_label.shape); print(df_label)
    
    # index
    index = 0
    for blob in generator:
        print(blob.name)
       # populate dataframe
        df.loc[index,'Category'] = df_label.loc[index,'category']
        df.loc[index,'Url'] = imageurl = "https://" + STORAGE_ACCOUNT_NAME + ".blob.core.windows.net/" + CONTAINER_NAME + "/" + blob.name; print(imageurl)
        index = index + 1
            
    # write dataframe to blob
    print("-----------------------")
    df_str = df.to_csv(sep='\t', index=False); 
    
    dfblobname = 'image-url.tsv' 
    block_blob_service.create_blob_from_text(CONTAINER_NAME_METADATA, dfblobname, df_str) 

    return
    
if __name__ == '__main__':
    main() 