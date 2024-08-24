# -*- coding: utf-8 -*-
"""
Created on Sun Jan 22 21:46:19 2023
Baby name popularity ranking (Liang 13.17)
@author: Lill-Kristin Karlsen
"""
from requests import get
years = ["2001", "2002", "2003", "2004", "2005",
"2006", "2007", "2008", "2009", "2010"]

website = "liveexample.pearsoncmg.com/data/babynameranking"

# An exception is thrown if the entered year is not between 2001 and 2010.
while True:
    try:
        year = input("Please enter a year between 2001 and 2010: ")
        if year in years:
            break
        else:
            raise TypeError
    except TypeError:
        print("Enter valid year between 2001 and 2010.")

# Fetching textfile with babynames according to year.
def fetch_data():
    url = "https://" + website + year + ".txt"
    data = get(url)
    textfile = data.text.lower()
    return textfile

# An exception is thrown if the entered gender is not 'f' or 'm'.
while True:
    try:
        gender = input(
        "Please enter a gender - 'f' for female or 'm' for male: ").lower()
        if gender == "f" or gender == "m":
          break
        else:
            raise TypeError
    except TypeError:
        print("Please enter 'f' or 'm' as the gender")

# An exception is thrown if the entered name is not present in the list.
while True:
    try:
        name = input("Please enter a babyname: ").lower()
        if name in fetch_data():
            break
        else:
            raise TypeError
    except TypeError:
        print(f"{name}.capitalize() is not present in list of babynames.")

# Splitting lines in textfile, searching for desired name and appending them to list.
def populating_list():
    extracted_name = []
    for line in fetch_data().splitlines():
        if name in line:
            extracted_name.append(line)
    return extracted_name

# Removing white spaces in the strings. Removing tabulators and splitting items.
def clean_data():
    no_spaces = [x.replace(' ', '') for x in populating_list()]
    clean_name = [x.split("\t") for x in no_spaces]
    return clean_name

# Using a loop to check all the lists for the exact name, to remove any partial name matches.
def exact_match():
    for lists in clean_data():
        if name in lists:
            final_result = lists
    return final_result

# Printing the correctly ranked babyname according to year and gender.
def main():
    if gender == 'f':
        print(f"Girl name {name.capitalize()} is ranked #{exact_match()[0]} in year {year}.")
    
    elif gender == 'm':
        print(f"Boy name {name.capitalize()} is ranked #{exact_match()[0]} in year {year}.")

if __name__ == '__main__':
    main()