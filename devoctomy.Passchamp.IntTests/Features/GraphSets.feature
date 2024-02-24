Feature: GraphSetsCompatability

Test compatability between all minor versions of graph sets for version compatability as well as historial decryption tests.

@graph @graphsets @compatability
Scenario: 01) Load all graph sets and test new encrypt and decrypt flows
	Given All core services registered to service provider
	And a sample random vault
	When run encryption tests:
		| SetId                                | Version | Password    |
		| E17E1F18-16CC-4DD8-8FB8-1AAF0153168D | 1.0     | password123 |
	And run decryption tests:
		| SetId                                | Version | Password    |
		| E17E1F18-16CC-4DD8-8FB8-1AAF0153168D | 1.0     | password123 |
	Then decryption test results match input:
		| SetId                                | Version |
		| E17E1F18-16CC-4DD8-8FB8-1AAF0153168D | 1.0     |		

@graph @graphsets @compatability
Scenario: 02) Load all graph sets and test decrypt flow against static test data
	Given All core services registered to service provider
	When run static decryption tests:
		| SetId                                | Version | Password    |
		| E17E1F18-16CC-4DD8-8FB8-1AAF0153168D | 1.0     | password123 |
	Then static decryption test results matches expected:
		| SetId                                | Version | InputPath              |
		| E17E1F18-16CC-4DD8-8FB8-1AAF0153168D | 1.0     | Tests/Input/Test1.json |

# TODO: Static tests should also test decrypt against all lower minor versions of the same set

