Feature: Graph

Method of building up many small actions into a larger, more complex task.

Scenario: Encrypt string using a password
	Given A new dictionary of nodes
	And RandomByteGeneratorNode named <SaltGeneratorNodeName> with a length of <SaltLength> and NextKey of <IvGeneratorNodeName>
	And RandomByteGeneratorNode named <IvGeneratorNodeName> with a length of <IvLength>  and NextKey of <DeriveKeyNodeName>
	And DeriveKeyFromPasswordNode named <DeriveKeyNodeName> with a password of <Password> and key length of <KeyLength> and NextKey of <Utf8EncoderNodeName>
	And Utf8EncoderNode named <Utf8EncoderNodeName> and NextKey of <EncryptNodeName>
	And EncryptNode named <EncryptNodeName> and NextKey of <JoinerNodeName>
	And ArrayJoinerNode named <JoinerNodeName>
	And A new graph with a start node named <SaltGeneratorNodeName>
	When Execute graph
	Then Something

	Examples:

	| SaltGeneratorNodeName | SaltLength | IvGeneratorNodeName | IvLength | DeriveKeyNodeName | Password | KeyLength | Utf8EncoderNodeName | EncryptNodeName | JoinerNodeName |
	| saltgenerator         | 16         | ivgenerator         | 16       | derivekey         | 123      | 32        | encoder             | encrypt         | joiner         |