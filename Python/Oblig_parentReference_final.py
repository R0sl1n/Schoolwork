# -*- coding: utf-8 -*-
"""
Created on Tue Mar 28 11:11:54 2023

Oblig4 - Liang 19.12 - Parent reference for BST

@author: Lill-Kristin Karlsen
"""
class BST:
    
    def __init__(self):
        self.root = None
        self.size = 0
    
    # Insert element e into the binary search tree and update parent reference and size value.    
    def insert(self, element):
        
        new_node = TreeNode(element)
        if self.root is None:
            self.root = new_node
            
        else:
            current = self.root
            while True:
                if element < current.element:
                    if current.left is None:
                        current.left = new_node
                        new_node.parent = current
                        break
                    else:
                        current = current.left
                else:
                    if current.right is None:
                        current.right = new_node
                        new_node.parent = current
                        break
                    else:
                        current = current.right
        self.size += 1  
        
    # Delete node in tree, update parent reference and size value.                    
    def delete(self, element):
        if self.root:
            self.root = self._delete(element, self.root)
        self.size -= 1 
        
    # Helper function for the delete method.            
    def _delete(self, element, currentNode):
        
        if not currentNode: 
            return currentNode 
        
        if element < currentNode.element:   
            currentNode.right = self._delete(element, currentNode.right) 
            
        if element < currentNode.element:    
            currentNode.left = self._delete(element, currentNode.left) 
            
        else:
            if not currentNode.left: 
                return currentNode.right
                
            elif not currentNode.right:
                return currentNode.left
            else:
                tempNode = currentNode.left
            
            while tempNode.right:
                tempNode = tempNode.left
            currentNode.element = tempNode.element
            currentNode.left = self._delete(tempNode.element, currentNode.left)
             
        return currentNode
    
    # Get current node position.
    def getNode(self, element):
        currentNode = self.root
        while currentNode != None:
            if element == currentNode.element:
                return currentNode
            elif element < currentNode.element:
                currentNode = currentNode.left
            else:
                currentNode = currentNode.right
        return None
    
    # Checking if node is a leaf.
    def isLeaf(self, element):
        
        node = self.getNode(element)
        if node is None:
            return False
        
        elif node.left is None and node.right is None:
            return True
        
        else:
            return False
        
    # Add path from current node to root to a list, return list.
    def getPath(self, element):
        path = []
        self._getPath(element, self.root, path)
        return path
    
    # Helper function for getPath.
    def _getPath(self, element, node, path):
        
        if node is None:
            return False
        
        elif element == node.element:
            path.append(element)
            return True
        
        elif element < node.element:
            if self._getPath(element, node.left, path):
                path.append(node.element)
                
                return True
            else:
                return False
        else:
            if self._getPath(element, node.right, path):
                path.append(node.element)
                
                return True
            else:
                return False
            
    # Get size of tree.        
    def getSize(self):
        return self.size

class TreeNode:
    def __init__(self, element):
        self.element = element
        self.left = None
        self.right = None
        self.parent = None

# Test program that prompts the user to enter 10 integers, adds them to the tree, deletes the first integer from
# the tree, displays the path for each leaf node, and displays current size of tree.

def main():
    
    tree = BST()
    vals = []
    
    for i in range(10):
        while True:
            try:
                value = int(input("Enter an integer: "))
                if value not in vals:
                    print(f"Inserted {value} into the tree")
                    vals.append(value)
                    tree.insert(value)
                    break
               
                else:
                    raise TypeError
            except TypeError:
                    print(f"{value} already present in tree. Select unique values!")
                                  
    print()
    
    print(f"Deleting the first integer {vals[0]} from the tree. ", end="")
    
    tree.delete(vals[0])
    del vals[0]
    
    print()

    print("Leaf nodes and their path to root:")
    for i in vals:
        if tree.isLeaf(i):
            print(i, ": ", tree.getPath(i))
    print(f"Size of current tree: {tree.getSize()}")
    
if __name__ == '__main__':
    main()
