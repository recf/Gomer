﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Gomer.Cli.Exceptions;
using Gomer.Core;
using ManyConsole;

namespace Gomer.Cli.Commands
{
    public class AddCommand : BaseCommand
    {
        private IList<string> _genres;

        private int _hours;

        private int _priority;

        private DateTime _addedDate;

        private string _alias;

        public AddCommand()
        {
            IsCommand("add", "Add a pile game.");

            _addedDate = DateTime.Today;
            _priority = 2;
            _hours = 10;
            _genres = new List<string>();

            Arg(
                "added-date",
                "{{DATE}} acquired. (default: {0:}))",
                v => _addedDate = v,
                'a',
                _addedDate);

            Arg(
                "priority",
                "{{PRIORITY}} of the game. (default: {0})",
                v => _priority = v,
                'p',
                _priority);

            Arg(
                "hours",
                "Estimated {{HOURS}} to complete. (default: {0})",
                v => _hours = v,
                'H',
                _hours);


            Arg(
                "tag", 
                "{{TAG}} that the game belongs to. Can be specified multiple times.", 
                v => _genres.Add(v),
                't');

            Arg(
                "alias", 
                "Alternate {{ALIAS}} for the game.", 
                v => _alias = v);

            HasAdditionalArguments(2, "<name> <platform>");
        }

        public override void Run(string[] remainingArguments, TextWriter output)
        {
            var pile = ReadFile();

            var name = remainingArguments[0];
            var platform = remainingArguments[1];

            if (pile.Games.Any(g => String.Equals(g.Name, name, StringComparison.CurrentCultureIgnoreCase)))
            {
                throw new CommandException("A game with that name already exists.");
            }

            var game = new PileGame
            {
                Name = name,
                Platform = platform,
                EstimatedHours = _hours,
                Priority = _priority,
                AddedDate = _addedDate,
                Tags = _genres
            };

            pile.Games.Add(game);

            Show(game, output);

            WriteFile(pile, output);
        }
    }
}
