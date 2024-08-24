# -*- coding: utf-8 -*-
"""
Created on Mon Feb  6 08:16:27 2023

Vehicle del1 (2023) - Oblig 1.

@author: Lill-Kristin Karlsen
"""

class Vehicle():
    
    def __init__(self, make, year, mileage, price):
        self.make = make
        self.year = year
        self.mileage = mileage
        self.price = price
        
    def get_make(self):
        return self.make # Get make
    
    def get_year(self):
        return self.year

    def get_mileage(self):
        return self.mileage #Get mileage

    def get_price(self):
        return self.price

    def set_make(self, make):
        self.make = make
    
    def set_year(self, year):
        self.year = year
    
    def set_mileage(self, mileage):
        self.mileage = mileage
        
    def set_price(self, price):
        self.price = price
        
    def __str__(self):
      return f"Make: {self.make}, Year: {self.year}, Mileage: {self.mileage}, Price: {self.price}"
  
class Car(Vehicle):
    
    def __init__(self, make, year, mileage, price, doors):
        super().__init__(make, year, mileage, price)  # Invoke superclass's init method
        self.doors = doors
        
    def get_doors(self):
        return self.doors
    
    def set_doors(self, doors):
        self.doors = doors
    
    def __str__(self):
       return f"Make: {self.make}, Year: {self.year}, Milage: {self.mileage}, Price: {self.price}, Doors: {self.doors}"
        
#Truck is a subclass of Vehicle.  
class Truck(Vehicle):   
    
    def __init__(self, make, year, mileage, price, wheels):
        super().__init__(make, year, mileage, price)  # Invoke superclass's init method
        self.wheels = wheels
        
    def get_drive_type(self):
        return self.wheels
    
    def set_drive_type(self, wheels):
        self.wheels = wheels
        
    def __str__(self):
          return f"Make: {self.make}, Year: {self.year}, Milage: {self.mileage}, Price: {self.price}, Wheels: {self.wheels}"
            

#SUV is a subclass of Vehicle. 
class SUV(Vehicle):
    
    def __init__(self, make, year, mileage, price, passengers):
        super().__init__(make, year, mileage, price)  # Invoke superclass's init method
        self.passengers = passengers
        
    def get_pass_cap(self):
        return self.passengers
    
    def set_pass_cap(self, passengers):
        self.passengers = passengers
        
    def __str__(self):
           return f"Make: {self.make}, Year: {self.year}, Milage: {self.mileage}, Price: {self.price}, Passengers: {self.passengers}"
        


    






        
        
        