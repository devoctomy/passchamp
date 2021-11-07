Feature: CommandLineParser

Simple service for parsing command line arguments.

@mytag
Scenario: 01) Correctly parse arguments string to options instance
	Given the arguments <Arguments>
	And options is of type <OptionsType>
	And command line parser service is a default instance
	When TryParseArgumentsAsOptions
	Then parsing was successful
	Then the options should match that of <ExpectedOptionsFile>

	Examples:

	| Case | Arguments                                       | OptionsType                                                        | ExpectedOptionsFile          |
	| 1    | helloworld -b=true -i=2 -f=5.55 -o=pants        | devoctomy.Passchamp.SignTool.IntTests.Steps.CommandLineTestOptions | Data/01-parse-options_1.json |
	| 2    | -s=helloworld --b=true --i=2 --f=5.55 --o=pants | devoctomy.Passchamp.SignTool.IntTests.Steps.CommandLineTestOptions | Data/01-parse-options_2.json |
