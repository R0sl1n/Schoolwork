# -*- coding: utf-8 -*-
"""
Created on Wed Apr 21 09:47:59 2021

Semesteroppgave 1 - INF621 - oppgave 3.

@author: Lill-Kristin Karlsen
"""
#3.a
"""I funksjonen les_krypteringsordbok leser den innholdet i hemmelig.txt og returnerer
en ordbok bestående av nøkler og verdier angitt i filen. """

def les_krypteringsordbok():
    
    with open("hemmelig.txt", "r") as fil:
        
        kryptordbok = {}
        
        for linjer in fil:
            
            (key, val) = linjer.strip().split(" erstattes med ")
        
            kryptordbok[key] = val
                
        return kryptordbok

#3.b
"""I funksjonen krypter(filnavn, krypteringsordbok) leser den først en fil angitt som
parameter i funksjonen, og skriver til en ny fil der innholdet er byttet ut med verdiene
fra innholdet i ordboken returnert av funksjonen les_krypteringsordbok"""
        
def krypter(filnavn, krypteringsordbok):
        
    with open(filnavn, "r") as fil, open("kryptert_" + filnavn, "w") as fil2:
        
        fil2_innhold = ""
        
        for linje in fil:
            for words in linje:
                for x in words:
                    if x in krypteringsordbok:
                        x = krypteringsordbok[x]
                        fil2_innhold += x
                    else:
                        fil2_innhold += x
                    
        fil2.write(fil2_innhold)
                
                        
 
#3.c 
"""I funksjonen dekrypter(filnavn, krypteringsordbok) skal man dekryptere den
krypterte filen man opprettet i funksjonen krypter(filnavn, krypteringsordbok) og 
skrive innholdet til en ny fil."""
        
def dekrypter(filnavn, krypteringsordbok):
    
     with open(filnavn, "r") as fil, open("dekryptert_" + filnavn, "w") as fil2:
         
         fil2_innhold = ""
         
         omvendte_verdier = {}
        
         for k, v in krypteringsordbok.items():
            omvendte_verdier[v] = k
            #print(omvendte_verdier) - Tester for å se at v, k har byttet plass.
         for linje in fil:
             for words in linje:
                 for x in words:
                     if x in omvendte_verdier.keys():
                        x = omvendte_verdier[x]
                        fil2_innhold += x
                     else:
                         fil2_innhold += x
                        
         fil2.write(fil2_innhold)
             
       

        
