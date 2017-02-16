# -*- coding: utf-8 -*-
"""
Created on Tue Nov  8 04:41:15 2016

@author: chyam
purpose: data re-labelling to reduce grouping.
"""

from __future__ import print_function
from azure.storage.blob import BlockBlobService
from azure.storage.blob import ContentSettings
import configparser
import pandas as pd
from io import StringIO

def main():
    
    # Get credential
    parser = configparser.ConfigParser()
    parser.read('config.ini')
    STORAGE_ACCOUNT_NAME = parser.get('credential', 'STORAGE_ACCOUNT_NAME')    
    STORAGE_ACCOUNT_KEY = parser.get('credential', 'STORAGE_ACCOUNT_KEY')
    CONTAINER_NAME_METADATA = parser.get('credential', 'CONTAINER_NAME_METADATA')

    # access to blob storage
    block_blob_service = BlockBlobService(account_name=STORAGE_ACCOUNT_NAME, account_key=STORAGE_ACCOUNT_KEY)

    # load structured data from blob
    blob_text = block_blob_service.get_blob_to_text(CONTAINER_NAME_METADATA, 'image-url.tsv');  #print(blob_text.content)
    df = pd.DataFrame.from_csv(StringIO(blob_text.content), index_col=None, sep='\t'); print(df.shape)
    
    # Frequency
    df_crosstab = pd.crosstab(index=df["Category"], columns="count")
    print(df_crosstab)
    
    print(df.shape)
    df_sub = pd.concat([df.loc[df['Category'] == 'daily snack'], 
           df.loc[df['Category'] == 'groceries'], 
           df.loc[df['Category'] == 'dining out'], 
           df.loc[df['Category'] == 'clothes and accessories'], 
           df.loc[df['Category'] == 'fuel'], 
           df.loc[df['Category'] == 'entertinement']])
    print(df_sub.shape)
    print(df_sub.Category.unique())
        
    # write cleaned dataframe to blob
    print("-----------write cleaned dataframe to blob------------")
    df_sub_str = df_sub.to_csv(sep='\t', index=False); 
    
    #dfblobname = 'dataframe_6_classes.tsv' 
    dfblobname = 'image-url-6-classes.tsv' 
    settings = ContentSettings(content_type='text/tab-separated-values')
    block_blob_service.create_blob_from_text(CONTAINER_NAME_METADATA, dfblobname, df_sub_str, content_settings=settings)
   
    return

if __name__ == '__main__':
    main() 