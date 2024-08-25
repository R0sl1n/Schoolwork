# -*- coding: utf-8 -*-
"""
Created on Sat Mar 11 11:40:13 2023

Ligningsbanken - Oblig.

@author: Lill-Kristin Karlsen
"""
#Importing library to create random numbers.
 
from random import randint

#main function calling other functions using specified arguments.
def main():

 tests = make_test(['Ola', 'Kari', 'Fredrik'], 5)

 answer_questions(tests)

 print(tests)
                        
#Taking a list of 4 ints as input, taking into consideration -1/1 and +- in regards to handling operators with neg. numbers.
# Returning a string with the syntax of the desired equation.

def eq2text(lst):
    
    a, b, c, d = lst
    
    eq = ""
    
    if a == 1:
        eq += "x"
        
    elif a == -1:
        eq += "-x"
        
    else:
        eq += str(a) + "x"
    
    if b < 0:
        eq += f" - {-b} = "
    else:
        eq += f" + {b} = "
        
    if c == 1:
        eq += "x"
        
    elif c == -1:
        eq += "-x"
        
    else:
        eq += str(c) + "x"
    
    if d < 0:
        eq += f" - {-d}"
        
    else:
        eq += f" + {d}"
    
    return eq

#Checking for 0 values, if first/third and second/fourth element are identical. Returning True if cond. are met.
  
def ok(L):
    
    if 0 in L:
        return False
    if L[0] == L[2]:
        return False
    if L[1] == L[3]:
        return False
    else:
        return True
    
#Making a list of 4 ints in range -9/9. Appending to a list. Checking list in regards to cond. using ok(L) func.
# If cond. are false, a new list is generated using a while loop to check True/False.

def make_eq():
    
    eq_list = []
    
    for num in range(1,5):
        num = randint(-9, 9)
        eq_list.append(num)
    
    test_num = ok(eq_list)
    
    while test_num == False:
        eq_list = []
        for num in range(1,5):
            num = randint(-9, 9)
            eq_list.append(num)
        test_num = ok(eq_list)
        
        if test_num == False:
            continue
        if test_num == True:
            break
    return eq_list
           
#Making n number of equations using a counter and while loop. Checking for duplicates by comparing length of set(list) and original list.
#Also checking if ab = cd in each list, if any of these cond. are met, new lists of equations are made. 
      
def make_n_eqs(n):
    
    n_eqs = []
    eq = make_eq()
    count = 1
    while count <=n:
        n_eqs.append(eq)
        count +=1
        eq = make_eq()
    
    unique = set()
    for items in n_eqs:
        unique.update(items)
   
    for a,b,c,d in n_eqs:
        if a == c and b == d or len(unique) != len(n_eqs):
            n_eqs = []
            eq = make_eq()
            count = 1
            while count <=n:
                n_eqs.append(eq)
                count +=1
                eq = make_eq()
      
    return n_eqs
    
#Making n equations for each student in a list, and returning studentnames as keys and equation lists as values in a dict.  
    
def make_test(students, n):
    
    stud_eq = {}
   
    for name in students:
        stud_eq[name] = make_n_eqs(n)
            
    print(stud_eq)        
    return stud_eq
        
#Taking dict as argument, student name as input, checking if name exists in the dict keys, if not, a while loop asks for the name again.
#Student are asked to solve eqs given in the value list of their namekey in dict. Equations are given wrapped in the 
#eq2text(lst), which provides the student with the correct format for answering, and checking if the value is a number. It updates only the values of the respective namekey,
#and returns the changed key values with the answers they provided, along with the originial values for the other students
#in a new dictionary.

def answer_questions(D):
    
    answers = []
    
    while True:
        try:
            name = input("Please enter your first name: ")
            if name in D.keys():
                break
            else:
                raise TypeError
        except TypeError:
            print(f"{name} is not present in studentlist.")
    
    #number_eq = len(D[name])
    
    print("Please solve these equations: ")
    
    new_dict = {}
    for k,v in D.items():
        if k == name:
            new_dict[k] = v
    
    for v in new_dict[name]:
        print(eq2text(v))
        
        while True:
                try:
                    x = float(input("X = "))
                    if type(x) == float or type(x) == int :
                        break
                    else:
                        raise ValueError
                except ValueError:
                    print("X is not a number.")    
                    
        answers.append(x)
        
    for index, value in enumerate(answers):
                    
        new_dict[name][index].append(value)
            
    return new_dict
   
main()    
    

    

    