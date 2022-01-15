Feature: SignTool

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
