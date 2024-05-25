# Testing:
Powershell in executabil pentru a downloada si executa un cod raw de pe server: fail
Crearea unui payload folosind msfvenom(reverse-shell) si obfuscarea folosind ScareCrow: Bitdefender recunoaste, Windows Defender nu, aplicatia nu ruleaza
Powershell care adauga o cale la exclusion path: Succes!(TREBUIE RUN AS ADMIN)#
# Planuri:

Creeaza executabil care:
 a) Adauga drive-ul C: in exclusion path(succes)
 b) Porneste un proces adevarat(succes)
 c) Adauga virusul la startup(succes)
 d) Cauta explicit path-ul in care se poate afla virusul(ongoing)
 e) Arata "legit" (ongoing) 
 f) Nu este detected de windows(succes)

Polish the executable:
 a) Programul sa deschida o aplicatie care se potriveste cu imaginea (ongoing)
Optional:
 a) Sa nu fie nevoie de run as administrator pentru a functiona
 b) Sa mute executabilul in alta locatie si sa il multiplice(virus-like)

