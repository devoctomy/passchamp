﻿Feature: Graph

Method of building up many small actions into a larger, more complex task.

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

	Examples:

	| SaltGeneratorNodeName | SaltLength | IvGeneratorNodeName | IvLength | DeriveKeyNodeName | Password | KeyLength | PlainText    | Utf8EncoderNodeName | EncryptNodeName | JoinerNodeName | FileWriterNodeName | FileName      |
	| saltgenerator         | 16         | ivgenerator         | 16       | derivekey         | 123      | 32        | Hello World! | encoder             | encrypt         | joiner         | writer             | data/test.dat |

Scenario: 02) Decrypt file using a password to correct plain text
	Given A new dictionary of nodes
	And FileReaderNode named <FileReaderNodeName> with a filename of <FileName> and NextKey of <DataParserNodeName>
	And DataParserNode named <DataParserNodeName> with parser sections of <ParserSections> and NextKey of <DecryptNodeName>
	And DecryptNode named <DecryptNodeName>
	And Node <DataParserNodeName> input pin Bytes connected to node <FileReaderNodeName> output pin Bytes
	And All nodes added to a new graph with a start node named <FileReaderNodeName>
	When Execute graph
	Then Something
	And All nodes executed

	Examples:

	| FileReaderNodeName | FileName      | DataParserNodeName | ParserSections                   | DecryptNodeName |
	| reader             | data/test.dat | parser             | Iv,0,16;Cipher,16,~0;Salt,~16,~0 | decrypt         |