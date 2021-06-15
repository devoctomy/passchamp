Feature: Graph

Method of building up many small actions into a larger, more complex task.

Scenario: Encrypt string using a password
	Given A new dictionary of nodes
	And RandomByteGeneratorNode named <SaltGeneratorNodeName> with a length of <SaltLength> and NextKey of <IvGeneratorNodeName>
	And RandomByteGeneratorNode named <IvGeneratorNodeName> with a length of <IvLength>  and NextKey of <DeriveKeyNodeName>
	And DeriveKeyFromPasswordNode named <DeriveKeyNodeName> with a password of <Password> and key length of <KeyLength> and NextKey of <Utf8EncoderNodeName>
	And Utf8EncoderNode named <Utf8EncoderNodeName> with plain text of <PlainText> and NextKey of <EncryptNodeName>
	And EncryptNode named <EncryptNodeName> and NextKey of <JoinerNodeName>
	And ArrayJoinerNode named <JoinerNodeName>
	And node <DeriveKeyNodeName> input pin Salt connected to node <SaltGeneratorNodeName> output pin RandomBytes
	And node <EncryptNodeName> input pin PlainTextBytes connected to node <Utf8EncoderNodeName> output pin EncodedBytes
	And node <EncryptNodeName> input pin Iv connected to node <IvGeneratorNodeName> output pin RandomBytes
	And node <EncryptNodeName> input pin Key connected to node <DeriveKeyNodeName> output pin Key
	And node <JoinerNodeName> input pin Part1 connected to node <IvGeneratorNodeName> output pin RandomBytes
	And node <JoinerNodeName> input pin Part2 connected to node <EncryptNodeName> output pin EncryptedBytes
	And node <JoinerNodeName> input pin Part3 connected to node <SaltGeneratorNodeName> output pin RandomBytes
	And All nodes added to a new graph with a start node named <SaltGeneratorNodeName>
	When Execute graph
	Then Something

	Examples:

	| SaltGeneratorNodeName | SaltLength | IvGeneratorNodeName | IvLength | DeriveKeyNodeName | Password | KeyLength | PlainText     | Utf8EncoderNodeName | EncryptNodeName | JoinerNodeName |
	| saltgenerator         | 16         | ivgenerator         | 16       | derivekey         | 123      | 32        | Hello World!  | encoder             | encrypt         | joiner         |