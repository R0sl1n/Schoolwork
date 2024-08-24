# This program creates a Car object, a Truck object,
# and an SUV object.

import Vehicle
import pickle
import os.path


# Constants for the menu choices
NEW_CAR_CHOICE = 1
NEW_TRUCK_CHOICE = 2
NEW_SUV_CHOICE = 3
FIND_VEHICLE_CHOICE = 4
SHOW_VEHICLES_CHOICE = 5
QUIT_CHOICE = 6


def main():
    
    if os.path.isfile("vehicle.bin"):
        with open("vehicle.bin", "ab") as file:
            vehicle_list = pickle.load(file)              
        
    else:
        # Create empty list for vehicles
        vehicle_list = []
       
    
    # Create a Car object for a used 2001 BMW
    # with 70,000 miles, priced at $15,000, with
    # 4 doors.
       
    car = Vehicle.Car('BMW 320', 2001, 70000, 15000.0, 4)
    vehicle_list.append(str(car))
    
    # Create a Truck object for a used 2002
    # Toyota pickup with 40,000 miles, priced
    # at $12,000, with 4-wheel drive.
    truck = Vehicle.Truck('Toyota RAV4', 2002, 40000, 12000.0, '4WD')
    vehicle_list.append(str(truck))
    
    # Create an SUV object for a used 2000
    # Volvo with 30,000 miles, priced
    # at $18,500, with 5 passenger capacity.
    suv = Vehicle.SUV('Volvo XC60', 2010, 30000, 18500.0, 5)
    vehicle_list.append(str(suv))
    
        
    choice = 0
    while choice != QUIT_CHOICE:
        # display the menu.
        display_menu()
    
        # Get the user's choice.
        choice = int(input("Enter your choice: "))
        print()
    
        # Perform the selected action.
        if choice == NEW_CAR_CHOICE:
            print("Add a new car: ")
            
            try: 
                make = input("Make: ").capitalize()
                year = int(input("Year:"))
                mileage = float(input("Milage: "))
                price = float(input("Price: "))
                doors = int(input("Doors: "))
                vehicle_list.append(str(Vehicle.Car(make, year, mileage, price, doors)))
                print()
                
            except ValueError:
                print("Invalid input - try again.")
                pass
            
        elif choice == NEW_TRUCK_CHOICE:
            print("Add a new truck: ")
            try:
                make = input("Make: ").capitalize()
                year = int(input("Year:"))
                mileage = float(input("Milage: "))
                price = float(input("Price: "))
                wheels = input("2WD or 4WD: ")
                vehicle_list.append(str(Vehicle.Truck(make, year, mileage, price, wheels)))
                print()
                
            except ValueError:
                print("Invalid input - try again.")
                pass
            
        elif choice == NEW_SUV_CHOICE:
            print("Add a new SUV: ")
            try:
                make = input("Make: ").capitalize()
                year = int(input("Year:"))
                mileage = float(input("Milage: "))
                price = float(input("Price: "))
                passengers = int(input("Passenger capacity: "))
                vehicle_list.append(str(Vehicle.SUV(make, year, mileage, price, passengers)))
                print()
            
            except ValueError:
                print("Invalid input - try again.")
                pass
            
        elif choice == FIND_VEHICLE_CHOICE:
            
            find_vehicle = input("Name of vehicle: ").lower()
            
            for item in vehicle_list:
                if find_vehicle in item.make.lower():
                    print(item)
                    print()
                    break             
            else: 
             
                   print(f"{find_vehicle} is not in list")
                   print()
           
                    
        elif choice == SHOW_VEHICLES_CHOICE:
            #show all vehicles
            print("The list contains the following vehicles: ")
            for item in vehicle_list:
                print(item)
            print()
                
        elif choice == QUIT_CHOICE:

            print("Exiting the program...")
            continue    
        else:
           print("Invalid selection.")
        
    
    vehicle_list.sort()
    print(vehicle_list)
        
    with open("vehicle_sorted.bin", "wb") as file:
       pickle.dump(vehicle_list, file)
        

# The display_menu function displays a menu.
def display_menu():
    print('        MENU')
    print('1) New car')
    print('2) New truck')
    print('3) New SUV')
    print('4) Find vehicles by make')
    print('5) Show all vehicles')
    print('6) Quit')     

# Call the main function.
if __name__ == '__main__':
      main()