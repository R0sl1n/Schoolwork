# -*- coding: utf-8 -*-
"""
Created on Mon Feb 20 14:07:30 2023

@author: Lill-Kristin Karlsen
"""

from pathlib import Path

def main():

    # Prompt the user to enter two filenames:

    print("Enter two files to be used for comparison: ")
    print()
    
    #setting the variables to global for availability in other functions.
    
    global filename1, filename2 

    #Checking if the filenames exists.If not, keeping a while loop active.
    
    while True:
        try:
            
            filename1 = input("Enter filename 1: ").strip()
            filename2 = input("Enter filename 2: ").strip()
            path_to_file1 = filename1
            path_to_file2 = filename2
            path1 = Path(path_to_file1)
            path2 = Path(path_to_file2)

            if path1.is_file() and path2.is_file():
                break
            
            else: raise TypeError
            
        except TypeError:
            print("Please enter valid filenames.")
    
    #Calling functions.
    
    unique_file1(filename1)
    unique_file2(filename2)
    count_unique_words()
    union_of_files()
    common_words()
    all_unique_first()
    all_unique_second()
    all_unique_words()


#Reading the filename1 and splitting into words, adding the words into a set, returning the set.

def unique_file1(filename1):

        with open(filename1, "r") as file1:
             unique_words1 = set()
             for line in file1:
                 word = line.split()
                 unique_words1.update(word)
       
        return unique_words1
    
#Reading the filename2 and splitting into words, adding the words into a set, returning the set.
    
def unique_file2(filename2):           
       
        with open(filename2, "r") as file2:
             unique_words2 = set()
             for line in file2:
                 word = line.split()
                 unique_words2.update(word)
                 
        return(unique_words2)     
    
#Counting and returning the amount of unique words using len() function.

def count_unique_words():
    
    file1 = unique_file1(filename1)
    file2 = unique_file2(filename2)
    
    
    amount_1 = len(file1)
    amount_2 = len(file2)
    print(f"Total amount of unique words in {filename1} is {amount_1}")
    print(f"Total amount of unique words in {filename2} is {amount_2}")
        
    return amount_1, amount_2

#Combining all unique words into one set using union() set method.

def union_of_files():
    
    file1 = unique_file1(filename1)
    file2 = unique_file2(filename2)
    combined = file1.union(file2)
    print(f"Unique words in {filename1} and {filename2} are: {combined}")
    return combined

#Finding common words in both files using intersection set method.
            
def common_words():   
     
    file1 = unique_file1(filename1)
    file2 = unique_file2(filename2)
        
    common = file1.intersection(file2)
        
    print(f"Common words in both files are: {common}")
    return common
                              
#Finding words that are only unique in file1 using difference() set method.

def all_unique_first():
    
    file1 = unique_file1(filename1)
    file2 = unique_file2(filename2)
    
    diff1 = file1.difference(file2)
    print(f"Unique words in {filename1} are: {diff1}")
    return diff1

#Finding words that are only unique in file2 using difference() set method.

def all_unique_second():
    
    file1 = unique_file1(filename1)
    file2 = unique_file2(filename2)
    
    diff2 = file2.difference(file1)
    
    print(f"Unique words in {filename2} are: {diff2}")
    return diff2


#Finding all unique words in both files that are not in common using symmetric_difference() set method. 
   
def all_unique_words():
    
    file1 = unique_file1(filename1)
    file2 = unique_file2(filename2)
    
    all_unique = file1.symmetric_difference(file2)
    
    print(f"All different unique words in both files are: {all_unique}")
    return all_unique


#Calling the main function
    
if __name__ == '__main__':
  main()                           

