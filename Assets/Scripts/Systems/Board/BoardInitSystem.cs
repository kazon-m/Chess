﻿using Components;
using Components.Chess;
using Components.Events;
using Data;
using Data.Enums;
using Leopotam.Ecs;
using UnityEngine;

namespace Systems.Board
{
    public class BoardInitSystem : IEcsRunSystem
    {
        private readonly EcsFilter<OnCreateBoardEvent> _filter = null;
        private readonly EcsWorld _world = null;
        private ChessPreset _chessPreset;

        public void Run()
        {
            foreach(var i in _filter)
            {
                ref var boardEvent = ref _filter.Get1(i);
                
                var boardEntity = _world.NewEntity();
                ref var board = ref boardEntity.Get<BoardComponent>();

                board.matrix = new SquareComponent[8, 8];
                board.component = boardEvent.board;

                var isWhite = true;

                for(var x = 0; x < 8; x++)
                {
                    for(var y = 0; y < 8; y++)
                    {
                        var squareEntity = _world.NewEntity();
                        ref var square = ref squareEntity.Get<SquareComponent>();

                        square.isWhite = isWhite;
                        square.position = new Vector2Int(x, y);
                        square.component = Object.Instantiate(_chessPreset.SquarePrefab, board.component.transform);
                        square.component.color = isWhite ? _chessPreset.SquareWhiteColor : _chessPreset.SquareBlackColor;

                        board.matrix[x, y] = square;

                        isWhite = !isWhite;
                    }

                    isWhite = !isWhite;
                }
                
                foreach(var chessItem in _chessPreset.ChessList)
                {
                    foreach(var position in chessItem.whitePositions)
                    {
                        var square = board.matrix[position.x, position.y];
                        
                        var chessEntity = _world.NewEntity();
                        ref var chess = ref chessEntity.Get<ChessComponent>();
                        
                        chess.component = Object.Instantiate(_chessPreset.ChessPrefab, square.component.transform);
                        chess.component.sprite = chessItem.spriteWhite;
                        chess.type = chessItem.type;
                        chess.team = TeamType.White;
                    }
                            
                    foreach(var position in chessItem.blackPositions)
                    {
                        var square = board.matrix[position.x, position.y];
                        
                        var chessEntity = _world.NewEntity();
                        ref var chess = ref chessEntity.Get<ChessComponent>();
                        
                        chess.component = Object.Instantiate(_chessPreset.ChessPrefab, square.component.transform);
                        chess.component.sprite = chessItem.spriteBlack;
                        chess.type = chessItem.type;
                        chess.team = TeamType.Black;
                    }
                }
            }
        }
    }
}