﻿using Tweetinvi.Core.Interfaces.Models;
using Tweetinvi.Core.Interfaces.Parameters;
using Tweetinvi.Core.Interfaces.Parameters.QueryParameters;

namespace Tweetinvi.Logic.QueryParameters
{
    public class GetTweetsFromListQueryParameters : IGetTweetsFromListQueryParameters
    {
        public GetTweetsFromListQueryParameters(ITwitterListIdentifier listIdentifier, IGetTweetsFromListParameters parameters)
        {
            TwitterListIdentifier = listIdentifier;
            Parameters = parameters;
        }

        public ITwitterListIdentifier TwitterListIdentifier { get; private set; }
        public IGetTweetsFromListParameters Parameters { get; private set; }
    }
}