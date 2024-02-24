Feature: GraphSetsCompatability

Test compatability between all minor versions of graph sets for version compatability as well as historial decryption tests.

# Need to fix these tests to work with Vault

#@graph @graphsets @compatability
#Scenario: 01) Load all graph sets and test new encrypt and decrypt flows
#	Given All core services registered to service provider
#	When run encryption tests:
#		| SetId                                | Version | InputPath              | Password    |
#		| E17E1F18-16CC-4DD8-8FB8-1AAF0153168D | 1.0     | Tests/Input/Test1.json | password123 |
#	And run decryption tests:
#		| SetId                                | Version | Password    |
#		| E17E1F18-16CC-4DD8-8FB8-1AAF0153168D | 1.0     | password123 |
#	Then decryption test results plain text matches input:
#		| SetId                                | Version | InputPath              |
#		| E17E1F18-16CC-4DD8-8FB8-1AAF0153168D | 1.0     | Tests/Input/Test1.json |		
#
#@graph @graphsets @compatability
#Scenario: 02) Load all graph sets and test decrypt flow against static test data
#	Given All core services registered to service provider
#	When run static decryption tests:
#		| SetId                                | Version | Password    |
#		| E17E1F18-16CC-4DD8-8FB8-1AAF0153168D | 1.0     | password123 |
#	Then static decryption test results plain text matches input:
#		| SetId                                | Version | InputPath              |
#		| E17E1F18-16CC-4DD8-8FB8-1AAF0153168D | 1.0     | Tests/Input/Test1.json |

# TODO: Static tests should also test decrypt against all lower minor versions of the same set

