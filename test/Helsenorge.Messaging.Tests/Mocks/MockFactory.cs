﻿using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using Helsenorge.Messaging.Abstractions;

namespace Helsenorge.Messaging.Tests.Mocks
{
	internal class MockFactory : IMessagingFactory
	{
		public const int HelsenorgeHerId = 93238;
		public const int OtherHerId = 93252;

		public MockCommunicationParty Helsenorge { get; }
		public MockCommunicationParty OtherParty { get; }

		public Dictionary<string, List<IMessagingMessage>> Qeueues { get; } = new Dictionary<string, List<IMessagingMessage>>();
		
		public bool IsClosed => false;

		public MockFactory()
		{
			Helsenorge = new MockCommunicationParty(this, HelsenorgeHerId);
			OtherParty = new MockCommunicationParty(this, OtherHerId);
		}

		public void Close()
		{
		}

		public IMessagingReceiver CreateMessageReceiver(string id)
		{
			return new MockReceiver(this, id);
		}

		public IMessagingSender CreateMessageSender(string id)
		{
			return new MockSender(this, id);
		}

		public IMessagingMessage CreteMessage(Stream stream)
		{
			return new MockMessage(stream);
		}
	}

	internal class MockCommunicationParty
	{
		public MockCommunicationParty(MockFactory factory, int herId)
		{
			Asynchronous = new MockQueue($"{herId}_async");
			Synchronous = new MockQueue($"{herId}_sync");
			Error = new MockQueue($"{herId}_error");
			SynchronousReply = new MockQueue($"{herId}_syncreply");

			factory.Qeueues.Add(Asynchronous.Name, Asynchronous.Messages);
			factory.Qeueues.Add(Synchronous.Name, Synchronous.Messages);
			factory.Qeueues.Add(Error.Name, Error.Messages);
			factory.Qeueues.Add(SynchronousReply.Name, SynchronousReply.Messages);
		}

		public MockQueue Asynchronous { get; }
		public MockQueue Synchronous { get; }
		public MockQueue Error { get; }
		public MockQueue SynchronousReply { get; }

	}

	internal class MockQueue
	{
		public string Name { get; }
		public List<IMessagingMessage> Messages { get; }

		public MockQueue(string name)
		{
			Name = name;
			Messages = new List<IMessagingMessage>();
		}
	}
}