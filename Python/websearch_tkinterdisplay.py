# -*- coding: utf-8 -*-
"""
Created on Wed May 12 13:44:44 2021
Semesteroppgave 2 - INF621 - Oppgave 2.
@author: Lill-Kristin Karlsen
"""
from matplotlib import pyplot
import numpy as np
from requests import get
from tkinter import *

# 2.a
"""
Funksjonen tar som parametre en liste med søkeord, og en oppslagstabell (dict) over antall funn
av søkeordet i ulike aviser. Funksjonen lager et stolpediagram over antall funn av de ulike
søkeordene i hver av avisene. 

"""
def stolper(søkeord, funn):

    color_list = ["blue", "red", "green", "orange", "yellow", "pink"]
    avisnavn = list(funn.keys())

    data = funn.values()
    
    bredde = .8 / len(data)
    
    fig, ax = pyplot.subplots()
    
    for i, rad in enumerate(data):
      X = np.arange(len(rad))
      z = np.arange(len(søkeord))
      pyplot.bar(X + i * bredde-bredde, rad, label=avisnavn[i],
        width = bredde,
        color = color_list[i % len(color_list)])
    
    
    ax.set_ylabel("Antall forekomster av søkeord")
    ax.set_title("Oversikt over søkeord og antall funn i ulike aviser")
    ax.set_xticks(z)
    ax.set_xticklabels(søkeord, rotation=45)
    ax.legend()
    pyplot.show()
  
          
#2.b
"""
Funksjonen tar som parameter en liste med søkeord.Funksjonen går gjennom avisene VG,Dagbladet og BT,
og lager en oppslagstabell over antall funn av hvert søkeord i listen. Funksjonen kaller så opp funksjonen
i 2.a for en grafisk fremstilling av resultatet.
"""
def søk(søkeord):
    
    ordbok = {}
    websider = ["VG", "Dagbladet", "BT"]
    for webside in websider:
        url = "https://" + webside + ".no"
        data = get(url)
        tekstfil = data.text.lower()
        
        for words in søkeord:
            verdi = tekstfil.count(words) 
            
            if webside not in ordbok.keys():
                ordbok[webside] = [verdi]
                
            else:
                ordbok[webside].append(verdi)
    
    stolper(søkeord, ordbok)
    
#2.c
"""
Funksjonen lar brukeren velge ett eller flere søkeord fra en angitt liste i 
et tkinter-basert GUI. Brukerens valg blir lagret i en liste og sendt til 
funksjonen i oppgave 2.b.

"""

def start_søk():
    
    
    søke_liste = []
    søkeord = []
    words = liste.curselection()
    for i in words:
        op = liste.get(i)
        søke_liste.append(op)
    for val in søke_liste:    
        søkeord.append(val)
    søk(søkeord) 
       
    
vindu = Tk()
vindu.minsize(400, 200)
vindu.title("Søk i VG, Dagbladet og BT etter valgte søkeord")
ramme = Frame(vindu)
ramme.pack()
tekst = Label(ramme, text="Velg emne(r) fra liste og trykk på 'Start søk' for å søke i innhold fra VG, Dagbladet og BT")
tekst.pack()
liste = Listbox(ramme, height=5, selectmode = MULTIPLE) #denne skal kunne returnere en liste
liste.pack()

x = ["Verden","USA","Europa","Norge","Noreg","Oslo","Bergen","Nyhet","Sport","Ski","Fotball","Klima","Corona","Korona"]

for i in range(len(x)):
    x[i] = x[i].lower()
    
for item in range(len(x)): 
	liste.insert(END, x[item]) 
	
knapp = Button(ramme, text="Start søk", command=start_søk)
knapp.pack() 
vindu.mainloop()




