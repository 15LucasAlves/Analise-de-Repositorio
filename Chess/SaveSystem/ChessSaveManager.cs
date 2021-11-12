using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace Chess
{
    class ChessSaveManager
    {
        private ChessGameManager _gameManager;

        public ChessSaveManager(ChessGameManager gameManager)
        {
            _gameManager = gameManager;
        }

        public void Save(string path)
        {
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream stream = new FileStream(path, FileMode.Create);

            ChessSaveData data = new ChessSaveData(_gameManager.MoveManager.GetMovesSnapshot());

            formatter.Serialize(stream, data);
            stream.Close();
        }
    }
}
