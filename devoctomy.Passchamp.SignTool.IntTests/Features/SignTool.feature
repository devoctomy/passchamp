﻿Feature: SignTool

RSA Key pair generation

@Generate
Scenario: 01) Generate a key pair
	Given generate command and no options
	When run
	Then private key file generated of 867 bytes
	And public key file generated of 267 bytes

Scenario: 02) Generate a 1024 bit key pair
	Given generate command and key length of 1024
	When run
	Then private key file generated of 867 bytes
	And public key file generated of 267 bytes

Scenario: 03) Generate a 2048 bit key pair
	Given generate command and key length of 2048
	When run
	Then private key file generated of 1631 bytes
	And public key file generated of 439 bytes

Scenario: 04) Generate a 3072 bit key pair
	Given generate command and key length of 3072
	When run
	Then private key file generated of 2387 bytes
	And public key file generated of 607 bytes

Scenario: 05) Generate a 4096 bit key pair
	Given generate command and key length of 4096
	When run
	Then private key file generated of 3171 bytes
	And public key file generated of 779 bytes

@Sign
Scenario: 01) Sign a JSON file
	Given sign command
	And private key file of "<PrivateKeyFile>"
	And input file of "<InputJsonFile>"
	And output filename of "<OutputFileName>"
	When run
	Then signature present in json

	Examples: 

	| Case | InputJsonFile                     | PrivateKeyFile            | OutputFileName |
	| 1    | Data/json/unsigned/unsigned1.json | Data/keys/privatekey.json | signed.json    |

@Verify
Scenario: 02) Verify a signed JSON file
	Given verify command
	And public key file of "<PublicKeyFile>"
	And input file of "<InputJsonFile>"
	When run
	Then verify successful

	Examples: 

	| Case | InputJsonFile | PublicKeyFile            |
	| 1    | signed.json   | Data/keys/publickey.json |

@Verify
Scenario: 03) Verify a signed JSON file that's been modified
	Given verify command
	And public key file of "<PublicKeyFile>"
	And modify input file "<InputJsonFile>" and save as "<ModifiedInputJsonFile>"
	And input file of "<ModifiedInputJsonFile>"
	When run
	Then verify failed

	Examples: 

	| Case | InputJsonFile | ModifiedInputJsonFile | PublicKeyFile            |
	| 1    | signed.json   | modified.json         | Data/keys/publickey.json |
