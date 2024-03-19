As an user I want to pass Windows Defender antivirus checkers and analyzers through various methods:
	i)Receive file loader and encode it in a format that bypasses Windows Defender
	ii)Create gallery with images and insert virus code executable inside of image
	iii)Test upload 
	iv)Encrypt all shell code so it is hard to read by an user and avoid being flagged
	by antivirus
	1) Insert the executable inside of Images, which can let me effectively create a gallery with one
	"impostor image", which on click would launch the virus
	2) Custom encryption for virus code so another user or antivirus software
	 would have a hard time decrypting it
	-->Possible encryption algorithms: RSA, random permutations, caesar cypher remastered
	3) Encode the virus in a complex format than can bypass the analyzer of the antivirus so it 
	doesn't get matched with known signatures
	-->Possible encodings: Base64 encoding, AEL Library
	4) Possibly add a header file for the code that hides all malicious code from the antivirus
