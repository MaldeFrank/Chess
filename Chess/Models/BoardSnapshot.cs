using Force.DeepCloner;
namespace Chess.Models
{
    public class BoardSnapshot
    {
        private ChessBoard SavedBoardState;

        public BoardSnapshot(ChessBoard boardToSave)
        {
          
            SavedBoardState = boardToSave.DeepClone();
        }

        public void Restore(ChessBoard currentBoard)
        {
            var restoredState = SavedBoardState.DeepClone();

            currentBoard.BoardCells = restoredState.BoardCells;
            currentBoard.PossibleMoves = restoredState.PossibleMoves;
            currentBoard.Selected = null;
            currentBoard.WhiteKingPos = restoredState.WhiteKingPos;
            currentBoard.BlackKingPos = restoredState.BlackKingPos;
        }
    }
}