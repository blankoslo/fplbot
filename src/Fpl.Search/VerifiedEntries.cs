﻿using Fpl.Search.Models;
using System.Collections.Generic;

namespace Fpl.Search
{
    public class VerifiedEntries
    {
        public static readonly IDictionary<int, VerifiedEntryType> VerifiedEntriesMap = new Dictionary<int, VerifiedEntryType>
        {
            // Chess masters:
            { 76862, VerifiedEntryType.ChessMaster }, // Magnus Carlsen

            // Premier League Players:
            { 3651702, VerifiedEntryType.FootballerInPL }, // John McGinn
            { 3995410, VerifiedEntryType.FootballerInPL }, // James Justin
            { 2956964, VerifiedEntryType.FootballerInPL }, // Kyle Bartley
            { 4333448, VerifiedEntryType.FootballerInPL }, // Michael Hector
            { 3949083, VerifiedEntryType.FootballerInPL }, // Jason Steele
            { 2824860, VerifiedEntryType.FootballerInPL }, // Harry Maguire
            { 5630688, VerifiedEntryType.FootballerInPL }, // Liam Cooper
            { 976717, VerifiedEntryType.FootballerInPL }, // Patrick Bamford
            { 3994143, VerifiedEntryType.FootballerInPL }, // Adam Webster
            { 2317459, VerifiedEntryType.FootballerInPL }, // Charlie Austin
            { 3533650, VerifiedEntryType.FootballerInPL }, // Lee Grant
            { 3011137, VerifiedEntryType.FootballerInPL }, // Andy Robertson
            { 5197871, VerifiedEntryType.FootballerInPL }, // James Maddison
            { 4977097, VerifiedEntryType.FootballerInPL }, // Luke Ayling
            { 4431216, VerifiedEntryType.FootballerInPL }, // Alex McCarthy
            { 4869402, VerifiedEntryType.FootballerInPL }, // Christian Walton
            { 4667259, VerifiedEntryType.FootballerInPL }, // Conor Hourihane
            { 5621264, VerifiedEntryType.FootballerInPL }, // Leif Davis
            { 1026172, VerifiedEntryType.FootballerInPL }, // Trent Alexander-Arnold
            { 4501590, VerifiedEntryType.FootballerInPL }, // Jacob Murphy
            { 3204228, VerifiedEntryType.FootballerInPL }, // Tom Heaton
            { 4973262, VerifiedEntryType.FootballerInPL }, // Jacob Ramsey
            { 5689627, VerifiedEntryType.FootballerInPL }, // Kalvin Philips
            { 4627487, VerifiedEntryType.FootballerInPL }, // Jack Butland
            { 5664364, VerifiedEntryType.FootballerInPL }, // Pascal Struijk
            { 50383, VerifiedEntryType.FootballerInPL }, // Matt Targett
            { 5657419, VerifiedEntryType.FootballerInPL }, // Harrison Reed
            { 5091389, VerifiedEntryType.FootballerInPL }, // Sander Berge
            { 5639035, VerifiedEntryType.FootballerInPL }, // Bobby Decorva-Reid
            { 6523017, VerifiedEntryType.FootballerInPL }, // Marc Albrighton
            { 2073952, VerifiedEntryType.FootballerInPL }, // Jack Stephens
            { 3560071, VerifiedEntryType.FootballerInPL }, // Robert Snodgrass
            { 6297033, VerifiedEntryType.FootballerInPL }, // Ezgjan Alioski
            { 1605584, VerifiedEntryType.FootballerInPL }, // Conor Townsend
            { 3257654, VerifiedEntryType.FootballerInPL }, // Neil Taylor
            { 6824049, VerifiedEntryType.FootballerInPL }, // Hamza Choudhury
            { 5677809, VerifiedEntryType.FootballerInPL }, // Ian Poveda
            { 5797199, VerifiedEntryType.FootballerInPL }, // Alireza Jahanbakhsh
            { 5747167, VerifiedEntryType.FootballerInPL }, // Yves Bissouma
            { 4971020, VerifiedEntryType.FootballerInPL }, // Martin Odegaard

            // Non Premier League Footballers:
            { 3349042, VerifiedEntryType.Footballer }, // Nicklas Bendtner
            { 223800, VerifiedEntryType.Footballer }, // Tommy Smith
            { 3545669, VerifiedEntryType.Footballer }, // Marlon Pack
            { 4144997, VerifiedEntryType.Footballer }, // Bradley Dack
            { 2829457, VerifiedEntryType.Footballer }, // Rune Almenning Jarstein
            { 5100073, VerifiedEntryType.Footballer }, // Peter Crouch
            { 113765, VerifiedEntryType.Footballer }, // Micah Richards
            { 123970, VerifiedEntryType.Footballer }, // Jack Clarke
            { 4193516, VerifiedEntryType.Footballer }, // Jordan Rhodes
            { 3687973, VerifiedEntryType.Footballer }, // Adam Armstrong
            { 2561955, VerifiedEntryType.Footballer }, // Ryan Shawcross
            { 4575219, VerifiedEntryType.Footballer }, // Callum Elder
            { 4939203, VerifiedEntryType.Footballer }, // Angus Gunn
            { 3686444, VerifiedEntryType.Footballer }, // Jordan Hugill
            { 3485944, VerifiedEntryType.Footballer }, // James Forrest
            { 2081113, VerifiedEntryType.Footballer }, // Andy King
            { 4087810, VerifiedEntryType.Footballer }, // Paddy McNair
            { 3709903, VerifiedEntryType.Footballer }, // Keiran Dowell
            { 4444857, VerifiedEntryType.Footballer }, // Glenn Murray
            { 1522151, VerifiedEntryType.Footballer }, // Ben Gibson
            { 5089010, VerifiedEntryType.Footballer }, // James Chester
            { 4693521, VerifiedEntryType.Footballer }, // Scott Brown
            { 4129928, VerifiedEntryType.Footballer }, // Steven Fletcher
            { 2131164, VerifiedEntryType.Footballer }, // Chris Sutton
            { 96286, VerifiedEntryType.Footballer }, // Joe Jacobson
            { 1109828, VerifiedEntryType.Footballer }, // Ashley Fletcher
            { 2103717, VerifiedEntryType.Footballer }, // George Friend
            { 410350, VerifiedEntryType.Footballer }, // Darren Fletcher
            { 2617471, VerifiedEntryType.Footballer }, // Ben Foster
            { 3807657, VerifiedEntryType.Footballer }, // Joe Allen
            { 3862147, VerifiedEntryType.Footballer }, // Barry Bannan
            { 1910910, VerifiedEntryType.Footballer }, // Gabriel Agbonlahor
            { 3164912, VerifiedEntryType.Footballer }, // Shane Duffy
            { 4619250, VerifiedEntryType.Footballer }, // Tom Ince
            { 4836310, VerifiedEntryType.Footballer }, // Jonathan Leko
            { 32480, VerifiedEntryType.Footballer }, // Tom Cleverley
            { 5561377, VerifiedEntryType.Footballer }, // Izzy Brown
            { 2620831, VerifiedEntryType.Footballer }, // Adam Reach
            { 2291182, VerifiedEntryType.Footballer }, // Greg Taylor
            { 5130354, VerifiedEntryType.Footballer }, // Jonathan Walters
            { 1970225, VerifiedEntryType.Footballer }, // Tyrese Campbell
            { 4638812, VerifiedEntryType.Footballer }, // Cameron Dawson
            { 2436494, VerifiedEntryType.Footballer }, // Rory Delap
            { 2337480, VerifiedEntryType.Footballer }, // Troy Deeney
            { 4676481, VerifiedEntryType.Footballer }, // Tom Lees
            { 3818534, VerifiedEntryType.Footballer }, // Marcus Tavernier
            { 3038726, VerifiedEntryType.Footballer }, // Grant Hanley
            { 4210541, VerifiedEntryType.Footballer }, // Keiran Trippier
            { 4640635, VerifiedEntryType.Footballer }, // Liam Palmer
            { 5123943, VerifiedEntryType.Footballer }, // Danny Rose
            { 783564, VerifiedEntryType.Footballer }, // Anton Ferdinand
            { 4115531, VerifiedEntryType.Footballer }, // Josh Windass
            { 2493403, VerifiedEntryType.Footballer }, // Yan Dhanda
            { 2148095, VerifiedEntryType.Footballer }, // Greg Docherty
            { 4609159, VerifiedEntryType.Footballer }, // Sam Vokes
            { 6816181, VerifiedEntryType.Footballer }, // Danny Simpson
            { 3561126, VerifiedEntryType.Footballer }, // Britt Assombalonga
            { 4851068, VerifiedEntryType.Footballer }, // Stefan Johansen
            { 7189188, VerifiedEntryType.Footballer }, // Angel Gomes
            { 4923695, VerifiedEntryType.Footballer }, // Harlee Dean
            { 1438053, VerifiedEntryType.Footballer }, // Pål André Helland
            { 2647828, VerifiedEntryType.Footballer }, // Freddy dos Santos
            
            // Actors:
            { 414271, VerifiedEntryType.Actor }, // Nicolai Cleve Broch
            { 2790434, VerifiedEntryType.Actor }, // Jørgen Evensen
            { 6032211, VerifiedEntryType.Actor }, // Jakob Schøyen Andersen
            
            // TV Faces:
            { 2591008, VerifiedEntryType.TvFace }, // Erik Solbakken
            { 2531339, VerifiedEntryType.TvFace }, // Hasse Hope
            { 749903, VerifiedEntryType.TvFace }, // Rasmus Wold
            { 332502, VerifiedEntryType.TvFace }, // Morten Ramm
            { 3073, VerifiedEntryType.TvFace }, // Einar Tørnquist
            { 1108127, VerifiedEntryType.TvFace }, // Mathias Skarpaas
            { 960869, VerifiedEntryType.TvFace }, // Magnus Devold
            { 1293652, VerifiedEntryType.TvFace }, // Thomas Aune
            { 406336, VerifiedEntryType.TvFace }, // Emil Gukild
            { 3371413, VerifiedEntryType.TvFace }, // Espen PA Lervaag
            { 504680, VerifiedEntryType.TvFace }, // Nicolay Ramm
            
            // Podcasters:
            { 1387, VerifiedEntryType.Podcaster }, // Martin Sleipnes
            { 1390, VerifiedEntryType.Podcaster }, // Sigurd Eskeland
            { 11567, VerifiedEntryType.Podcaster }, // Kim Grina Midtlie
            { 101239, VerifiedEntryType.Podcaster }, // Christian Wessel Grundseth
            { 4163, VerifiedEntryType.Podcaster }, // Simen Aasum
            { 1506, VerifiedEntryType.Podcaster }, // Franco Roche
            { 455115, VerifiedEntryType.Podcaster }, // Sondre Golden Midtgarden

            // Athletes:
            { 241104, VerifiedEntryType.Athlete }, // Kjetil Jansrud
            { 588381, VerifiedEntryType.Athlete }, // Mats Zuccarello

            // CommunityFame:
            { 5225, VerifiedEntryType.CommunityFame }, // Mats Mauno (@FPLPiglet)
            { 55085, VerifiedEntryType.CommunityFame }, // Pranil Sheth (@lateriser12)
            { 132073, VerifiedEntryType.CommunityFame } // Ben Crellin (@bencrellin)
        };

        public static readonly IDictionary<int, int> VerifiedPLEntryToPlayerIdMap = new Dictionary<int, int>
            {
                {3651702, 38}, // John McGinn
                {3995410, 239}, // James Justin
                {2956964, 412}, // Kyle Bartley
                {4333448, 178}, // Michael Hector
                {3949083, 528}, // Jason Steele
                {2824860, 298}, // Harry Maguire
                {5630688, 195}, // Liam Cooper
                {976717,  202}, // Patrick Bamford
                {3994143, 66}, // Adam Webster
                {2317459, 415}, // Charlie Austin
                {3533650, 604}, // Lee Grant
                {3011137, 255}, // Andy Robertson
                {5197871, 231}, // James Maddison
                {4977097, 197}, // Luke Ayling
                {4431216, 363}, // Alex McCarthy
                {4869402, 503}, // Christian Walton
                {4667259, 33}, // Conor Hourihane
                {5621264, 214}, // Leif Davis
                {1026172, 259}, // Trent Alexander-Ar
                {4501590, 497}, // Jacob Murphy
                {3204228, 28}, // Tom Heaton
                {4973262, 554}, // Jacob Ramsey
                {5689627, 204}, // Kalvin Philips
                {4627487, 586}, // Jack Butland
                {5664364, 211}, // Pascal Struijk
                {50383,   42}, // Matt Targett
                {5657419, 373}, // Harrison Reed
                {5091389, 360}, // Sander Berge
                {5639035, 181}, // Bobby Decorva-Reid
                {6523017, 221}, // Marc Albrighton
                {2073952, 367}, // Jack Stephens
                {3560071, 428}, // Robert Snodgrass
                {6297033, 201}, // Ezgjan Alioski
                {1605584, 418}, // Conor Townsend
                {3257654, 31}, // Neil Taylor
                {6824049, 234}, // Hamza Choudhury
                {5677809, 209}, // Ian Poveda
                {5797199, 71}, // Alireza Jahanbakhs
                {5747167, 76}, // Yves Bissouma
                {4971020, 645}, // Martin Odegaard
            };
    }
}