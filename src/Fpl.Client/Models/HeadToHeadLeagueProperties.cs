﻿using System;
using System.Text.Json.Serialization;

namespace Fpl.Client.Models
{
    public class HeadToHeadLeagueProperties
    {
        [JsonPropertyName("id")]
        public int Id { get; set; }

        [JsonPropertyName("name")]
        public string Name { get; set; }

        [JsonPropertyName("has_started")]
        public bool HasStarted { get; set; }

        [JsonPropertyName("can_delete")]
        public bool CanDelete { get; set; }

        [JsonPropertyName("short_name")]
        public string ShortName { get; set; }

        [JsonPropertyName("created")]
        public DateTime CreatedAt { get; set; }

        [JsonPropertyName("closed")]
        public bool Closed { get; set; }

        [JsonPropertyName("forum_disabled")]
        public bool ForumDisabled { get; set; }

        [JsonPropertyName("make_code_public")]
        public bool MakeCodePublic { get; set; }

        [JsonPropertyName("rank")]
        public int? Rank { get; set; }

        [JsonPropertyName("size")]
        public int? Size { get; set; }

        [JsonPropertyName("league_type")]
        public string LeagueType { get; set; }

        [JsonPropertyName("_scoring")]
        public string Scoring { get; set; }

        [JsonPropertyName("ko_rounds")]
        public int ReprocessStandings { get; set; }

        [JsonPropertyName("admin_entry")]
        public int AdminEntry { get; set; }

        [JsonPropertyName("start_event")]
        public int StartEvent { get; set; }
    }
}
