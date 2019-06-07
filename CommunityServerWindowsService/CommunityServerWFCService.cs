using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CommunityServerWindowsService
{
    class CommunityServerWFCService : ICommunityServerWFCService
    {
        public int SubmitGame(GameDetails game)
        {
            return Service.engine.GameSubmitted(game);
        }

        public GameDetails RequestGame(GameDetails gameDetails)
        {
            return Service.engine.GameRequested(gameDetails);
        }
    }
}
