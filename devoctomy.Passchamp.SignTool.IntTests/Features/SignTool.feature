Feature: SignTool

A short summary of the feature

@tag1
Scenario: 01) Generate a key pair
	Given generate command and no options
	When run
	Then private key file generated
	And public key file generated
