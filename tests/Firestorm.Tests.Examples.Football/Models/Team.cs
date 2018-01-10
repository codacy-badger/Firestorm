﻿using System;
using System.Collections.Generic;
using System.Text;

namespace Firestorm.Tests.Examples.Football.Models
{
    public class Team
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public int FoundedYear { get; set; }

        public ICollection<Player> Players { get; set; }

        public League League { get; set; }

        public ICollection<FixtureTeam> Fixtures { get; set; }
    }
}