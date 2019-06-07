using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ServiceModel;

namespace CommunityServerWindowsService
{
    [ServiceContract(Namespace="http://myemulators/Service/v1",Name="CommunityServerWCFService")]
    public interface ICommunityServerWFCService
    {
        /// <summary>
        /// Client uses this function to submit a manually choosen game details.
        /// </summary>
        /// <param name="game">A game object filled with the details of the game.</param>
        /// <param name="submissionID">When a user is updating a games details, the client uses this to specifiy which subission the previous details were assoicated with.</param>
        [OperationContract(Name="SubmitGameDetails")]
        int SubmitGame(GameDetails game);

        /// <summary>
        /// Client uses this function to request details of a game with the subitted hash or filename.
        /// </summary>
        /// <param name="hash">The hash of the rom to look for data</param>
        /// <param name="filename">The filename of the rom to look for data</param>
        /// <returns></returns>
        [OperationContract(Name="RequestGameDetials")]
        GameDetails RequestGame(GameDetails gameDetails);
    }
}
