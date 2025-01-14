﻿using System;
using System.Threading.Tasks;
using Tweetinvi.Core.Helpers;
using Tweetinvi.Core.Interfaces;
using Tweetinvi.Core.Interfaces.Controllers;
using Tweetinvi.Core.Interfaces.DTO;
using Tweetinvi.Core.Interfaces.Factories;

namespace Tweetinvi.Logic
{
    /// <summary>
    /// Message that can be sent privately between Twitter users
    /// </summary>
    public class Message : IMessage
    {
        private readonly IUserFactory _userFactory;
        private readonly IMessageController _messageController;

        private IMessageDTO _messageDTO;
        private readonly ITaskFactory _taskFactory;
        private IUser _sender;
        private IUser _receiver;

        public Message(
            IUserFactory userFactory,
            IMessageController messageController,
            IMessageDTO messageDTO,
            ITaskFactory taskFactory)
        {
            _userFactory = userFactory;
            _messageController = messageController;
            _messageDTO = messageDTO;
            _taskFactory = taskFactory;
        }

        // Properties
        public IMessageDTO MessageDTO
        {
            get { return _messageDTO; }
            set { _messageDTO = value; }
        }

        public long Id
        {
            get { return _messageDTO.Id; }
        }

        public DateTime CreatedAt
        {
            get { return _messageDTO.CreatedAt; }
        }

        public long SenderId
        {
            get { return _messageDTO.SenderId; }
        }

        public string SenderScreenName
        {
            get { return _messageDTO.SenderScreenName; }
        }

        public IUser Sender
        {
            get
            {
                if (_sender == null)
                {
                    _sender = _userFactory.GenerateUserFromDTO(_messageDTO.Sender);
                }

                return _sender;
            }
        }

        public long ReceiverId
        {
            get { return _messageDTO.RecipientId; }
        }

        public string ReceiverScreenName
        {
            get { return _messageDTO.RecipientScreenName; }
        }

        public IUser Receiver
        {
            get
            {
                if (_receiver == null)
                {
                    _receiver = _userFactory.GenerateUserFromDTO(_messageDTO.Recipient);
                }

                return _receiver;
            }
        }

        public string Text
        {
            get { return _messageDTO.Text; }
        }

        public bool IsMessagePublished
        {
            get { return _messageDTO.IsMessagePublished; }
        }

        public bool IsMessageDestroyed
        {
            get { return _messageDTO.IsMessageDestroyed; }
        }

        // Publish
        public bool Publish()
        {
            var publishedMessage = _messageController.PublishMessage(_messageDTO);
            if (publishedMessage == null)
            {
                return false;
            }

            _messageDTO = publishedMessage.MessageDTO;
            return true;
        }

        public bool PublishTo(IUser recipient)
        {
            SetRecipient(recipient);
            return Publish();
        }

        // Destroy
        public bool Destroy()
        {
            return _messageController.DestroyMessage(_messageDTO);
        }

        // Set Recipient
        public void SetRecipient(IUser recipient)
        {
            _messageDTO.Recipient = recipient != null ? recipient.UserDTO : null;
        }

        public bool Equals(IMessage other)
        {
            bool result = 
                Id == other.Id && 
                Text == other.Text &&
                Sender.Equals(other.Sender) &&
                Receiver.Equals(other.Receiver);

            return result;
        }

        #region Async
        public async Task<bool> PublishAsync()
        {
            return await _taskFactory.ExecuteTaskAsync(() => Publish());
        }

        public async Task<bool> PublishToAsync(IUser recipient)
        {
            return await _taskFactory.ExecuteTaskAsync(() => PublishTo(recipient));
        }

        public async Task<bool> DestroyAsync()
        {
            return await _taskFactory.ExecuteTaskAsync(() => Destroy());
        } 
        #endregion

        public override string ToString()
        {
            return Text;
        }
    }
}