Feature: Graph

Method of building up many small actions into a larger, more complex task.

@graph
Scenario: 01) Encrypt string using a password and write to disk
	Given A new dictionary of nodes
	And RandomByteGeneratorNode named <SaltGeneratorNodeName> with a length of <SaltLength> and NextKey of <IvGeneratorNodeName>
	And RandomByteGeneratorNode named <IvGeneratorNodeName> with a length of <IvLength> and NextKey of <DeriveKeyNodeName>
	And DeriveKeyFromPasswordNode named <DeriveKeyNodeName> with a password of <Password> and key length of <KeyLength> and NextKey of <Utf8EncoderNodeName>
	And Utf8EncoderNode named <Utf8EncoderNodeName> with plain text of <PlainText> and NextKey of <EncryptNodeName>
	And EncryptNode named <EncryptNodeName> and NextKey of <JoinerNodeName>
	And ArrayJoinerNode named <JoinerNodeName> and NextKey of <FileWriterNodeName>
	And FileWriterNode named <FileWriterNodeName> with a filename of <FileName>
	And Node <DeriveKeyNodeName> input pin Salt connected to node <SaltGeneratorNodeName> output pin RandomBytes
	And Node <EncryptNodeName> input pin PlainTextBytes connected to node <Utf8EncoderNodeName> output pin EncodedBytes
	And Node <EncryptNodeName> input pin Iv connected to node <IvGeneratorNodeName> output pin RandomBytes
	And Node <EncryptNodeName> input pin Key connected to node <DeriveKeyNodeName> output pin Key
	And Node <JoinerNodeName> input pin Part1 connected to node <IvGeneratorNodeName> output pin RandomBytes
	And Node <JoinerNodeName> input pin Part2 connected to node <EncryptNodeName> output pin EncryptedBytes
	And Node <JoinerNodeName> input pin Part3 connected to node <SaltGeneratorNodeName> output pin RandomBytes
	And Node <FileWriterNodeName> input pin InputData connected to node <JoinerNodeName> output pin JoinedOutput
	And All nodes added to a new graph with a start node named <SaltGeneratorNodeName>
	When Execute graph
	Then Output file <FileName> created of 48 bytes in length
	And All nodes executed
	#And All all nodes executed in correct order of saltgenerator,ivgenerator,derive,encoder,encrypt,joiner,writer

	Examples:

	| SaltGeneratorNodeName | SaltLength | IvGeneratorNodeName | IvLength | DeriveKeyNodeName | Password | KeyLength | PlainText    | Utf8EncoderNodeName | EncryptNodeName | JoinerNodeName | FileWriterNodeName | FileName        |
	| saltgenerator         | 16         | ivgenerator         | 16       | derive            | 123      | 32        | Hello World! | encoder             | encrypt         | joiner         | writer             | Output/test.dat |

Scenario: 02) Decrypt file using a password to correct plain text
	Given A new dictionary of nodes
	And FileReaderNode named <FileReaderNodeName> with a filename of <FileName> and NextKey of <DataParserNodeName>
	And DataParserNode named <DataParserNodeName> with parser sections of <ParserSections> and NextKey of <DeriveKeyNodeName>
	And DeriveKeyFromPasswordNode named <DeriveKeyNodeName> with a password of <Password> and key length of <KeyLength> and NextKey of <DecryptNodeName>
	And DecryptNode named <DecryptNodeName> and NextKey of <DecoderNodeName>
	And Utf8DecoderNode named <DecoderNodeName>
	And Node <DataParserNodeName> input pin Bytes connected to node <FileReaderNodeName> output pin Bytes
	And Node <DeriveKeyNodeName> input pin <SaltSectionKey> connected to DataParserNode <DataParserNodeName> section <SaltSectionKey> value
	And Node <DecryptNodeName> input pin <IvSectionKey> connected to DataParserNode <DataParserNodeName> section <IvSectionKey> value
	And Node <DecryptNodeName> input pin <CipherSectionKey> connected to DataParserNode <DataParserNodeName> section <CipherSectionKey> value
	And Node <DecryptNodeName> input pin Key connected to node <DeriveKeyNodeName> output pin Key
	And Node <DecoderNodeName> input pin EncodedBytes connected to node <DecryptNodeName> output pin DecryptedBytes
	And All nodes added to a new graph with a start node named <FileReaderNodeName>
	When Execute graph
	Then Node <DecoderNodeName> output pin PlainText equals string <PlainText>
	And All nodes executed
	#And All all nodes executed in correct order of reader,parser,derivekey,decrypt,decode

	Examples:

	| FileReaderNodeName | FileName                | DataParserNodeName | ParserSections                    | DeriveKeyNodeName | Password | KeyLength | DecryptNodeName | IvSectionKey | CipherSectionKey | SaltSectionKey | DecoderNodeName | PlainText    |
	| reader             | Output/testdec.dat      | parser             | Iv,0,16;Cipher,16,~16;Salt,~16,~0 | derivekey         | 123      | 32        | decrypt         | Iv           | Cipher           | Salt           | decode          | Hello World! |

Scenario: 03) Encrypt string using a password with scrypt and write to disk
	Given A new dictionary of nodes
	And RandomByteGeneratorNode named <SaltGeneratorNodeName> with a length of <SaltLength> and NextKey of <IvGeneratorNodeName>
	And RandomByteGeneratorNode named <IvGeneratorNodeName> with a length of <IvLength> and NextKey of <DeriveKeyNodeName>
	And SCryptNode named <DeriveKeyNodeName> with a password of <Password> and NextKey of <Utf8EncoderNodeName>
	And Utf8EncoderNode named <Utf8EncoderNodeName> with plain text of <PlainText> and NextKey of <EncryptNodeName>
	And EncryptNode named <EncryptNodeName> and NextKey of <JoinerNodeName>
	And ArrayJoinerNode named <JoinerNodeName> and NextKey of <FileWriterNodeName>
	And FileWriterNode named <FileWriterNodeName> with a filename of <FileName>
	And Node <DeriveKeyNodeName> input pin Salt connected to node <SaltGeneratorNodeName> output pin RandomBytes
	And Node <EncryptNodeName> input pin PlainTextBytes connected to node <Utf8EncoderNodeName> output pin EncodedBytes
	And Node <EncryptNodeName> input pin Iv connected to node <IvGeneratorNodeName> output pin RandomBytes
	And Node <EncryptNodeName> input pin Key connected to node <DeriveKeyNodeName> output pin Key
	And Node <JoinerNodeName> input pin Part1 connected to node <IvGeneratorNodeName> output pin RandomBytes
	And Node <JoinerNodeName> input pin Part2 connected to node <EncryptNodeName> output pin EncryptedBytes
	And Node <JoinerNodeName> input pin Part3 connected to node <SaltGeneratorNodeName> output pin RandomBytes
	And Node <FileWriterNodeName> input pin InputData connected to node <JoinerNodeName> output pin JoinedOutput
	And All nodes added to a new graph with a start node named <SaltGeneratorNodeName>
	When Execute graph
	Then Output file <FileName> created of 48 bytes in length
	And All nodes executed
	#And All all nodes executed in correct order of saltgenerator,ivgenerator,derive,encoder,encrypt,joiner,writer

	Examples:

	| SaltGeneratorNodeName | SaltLength | IvGeneratorNodeName | IvLength | DeriveKeyNodeName | Password | KeyLength | PlainText    | Utf8EncoderNodeName | EncryptNodeName | JoinerNodeName | FileWriterNodeName | FileName             |
	| saltgenerator         | 16         | ivgenerator         | 16       | derive            | 123      | 32        | Hello World! | encoder             | encrypt         | joiner         | writer             | Output/test2.dat      |

Scenario: 04) Decrypt file using a password with scrypt to correct plain text
	Given A new dictionary of nodes
	And FileReaderNode named <FileReaderNodeName> with a filename of <FileName> and NextKey of <DataParserNodeName>
	And DataParserNode named <DataParserNodeName> with parser sections of <ParserSections> and NextKey of <DeriveKeyNodeName>
	And SCryptNode named <DeriveKeyNodeName> with a password of <Password> and NextKey of <DecryptNodeName>
	And DecryptNode named <DecryptNodeName> and NextKey of <DecoderNodeName>
	And Utf8DecoderNode named <DecoderNodeName>
	And Node <DataParserNodeName> input pin Bytes connected to node <FileReaderNodeName> output pin Bytes
	And Node <DeriveKeyNodeName> input pin Salt connected to DataParserNode <DataParserNodeName> section <SaltSectionKey> value
	And Node <DecryptNodeName> input pin <IvSectionKey> connected to DataParserNode <DataParserNodeName> section <IvSectionKey> value
	And Node <DecryptNodeName> input pin <CipherSectionKey> connected to DataParserNode <DataParserNodeName> section <CipherSectionKey> value
	And Node <DecryptNodeName> input pin Key connected to node <DeriveKeyNodeName> output pin Key
	And Node <DecoderNodeName> input pin EncodedBytes connected to node <DecryptNodeName> output pin DecryptedBytes
	And All nodes added to a new graph with a start node named <FileReaderNodeName>
	When Execute graph
	Then Node <DecoderNodeName> output pin PlainText equals string <PlainText>
	And All nodes executed
	#And All all nodes executed in correct order of reader,parser,derivekey,decrypt,decode

	Examples:

	| FileReaderNodeName | FileName             | DataParserNodeName | ParserSections                    | DeriveKeyNodeName | Password | KeyLength | DecryptNodeName | IvSectionKey | CipherSectionKey | SaltSectionKey | DecoderNodeName | PlainText    |
	| reader             | Output/test2.dat     | parser             | Iv,0,16;Cipher,16,~16;Salt,~16,~0 | derivekey         | 123      | 32        | decrypt         | Iv           | Cipher           | Salt           | decode          | Hello World! |