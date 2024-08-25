# -*- coding: utf-8 -*-
"""
Created on Sat Mar 25 17:35:26 2023

Oblig - Postfix notation Liang 18.7.

@author: Lill-Kristin Karlsen
"""

class Stack:
    def __init__(self):
        self.__elements = []

    # Return true if the stack is empty
    def isEmpty(self):
        return len(self.__elements) == 0
    
    # Returns the element at the top of the stack 
    # without removing it from the stack.
    def peek(self):
        if self.isEmpty():
            return None
        else:
            return self.__elements[len(self.__elements) - 1]

    # Stores an element into the top of the stack
    def push(self, value):
        self.__elements.append(value)

    # Removes the element at the top of the stack and returns it
    def pop(self):
        if self.isEmpty():
            return None
        else:
            return self.__elements.pop() 
        
    # Return the size of the stack
    def getSize(self):
        return len(self.__elements)


# Evaluate an expression 
def evaluateExpression(expression):
    
    # Create operandStack to store operands
    operandStack = Stack()

    # Splitting expression.
    tokens = expression.split()
    
    # Operatorlist.
    op = ["+", "-", "*" , "/"]
    
    result = None
    # Looping through items in tokens and pushing numbers to stack.
    for item in tokens:
        
        if item.isnumeric():
            operandStack.push(item)
            
    # If a operator is identified, it is appended to a list in between the 2 top numbers in the stack,
    # converted to a string and evaluated. The result is pushed back to the stack for further processing. End result is returned.
    
        if item in op:
            temp = [operandStack.pop()]
            temp.append(item)
            temp.append(operandStack.pop())
            string = "".join(str(x) for x in temp)
            result = eval(string)
            operandStack.push(result)
  
    return result



def main():
    expression = input("Enter an expression: ").strip()
    try:
        print(expression, "=", evaluateExpression(expression))
    except:
        print("Wrong expression: ", expression)
    evaluateExpression(expression)            

if __name__ == '__main__':
    main()