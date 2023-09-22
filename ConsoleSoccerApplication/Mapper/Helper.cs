using ConsoleSoccerApplication.Models.Dtos;
using ConsoleSoccerApplication.Models.Entities;
using System.Collections.Generic;
using System.Linq;

namespace ConsoleSoccerApplication.Mapper
{
    public static class Helper
    {
        public static IEnumerable<CompetitionDTO> GetAvailableCompetitions(IEnumerable<Competition> competitions)
        {
            //Preenche o DTO de informações da competição
            var competitionsDTO = competitions
                                ?.OrderByDescending(c => c.Name)
                                ?.Select(c =>
                                    new CompetitionDTO
                                    {
                                        CompetitionId = c.Id,
                                        CompetitionName = c.Name,
                                        CompetitionCode = c.Code,
                                        AreaName = c.Area.Name
                                    });

            return competitionsDTO;
        }

        public static IEnumerable<TeamDTO> GetTeams(IEnumerable<Team> teams)
        {
            //Preenche o DTO de informações do time
            var teamsDTO = teams
                                ?.OrderByDescending(t => t.Name)
                                ?.Select(t =>
                                    new TeamDTO
                                    {
                                        TeamId = t.Id,
                                        TeamName = t.Name,
                                        TeamShortName = t.ShortName,
                                        TeamTLA = t.TLA
                                    });

            return teamsDTO;
        }

        public static TeamInfoDTO GetTeamInfo(Team team)
        {
            //Preenche o DTO de informações mais detalhadas do time
            var teamInfoDTO = new TeamInfoDTO
            {
                Nationality = team.Area.Name,
                Name = team.Name,
                ShortName = team.ShortName,
                Founded = team.Founded.Value,
                ClubColors = team.ClubColors,
                Venue = team.Venue,
                Coach = new CoachDTO
                {
                    Name = team.Coach.Name,
                    Nationality = team.Coach.Nationality,
                    Contract = new Contract 
                    { 
                        Start = team.Coach.Contract.Start,
                        Until = team.Coach.Contract.Until
                    }
                },
                Squad = team.Squad?.Select(p => new PlayerDTO
                {
                    Id = p.Id,
                    Name = p.Name,
                    Age = p.CalcPlayerAge(),
                    Position = p.Position,
                    Nationality = p.Nationality
                }),
                Staff = team.Staff?.Select(c => new CoachDTO
                {
                    Name = c.Name,
                    Nationality = c.Nationality,
                    Contract = new Contract
                    {
                        Start = c.Contract.Start,
                        Until = c.Contract.Until
                    }
                })
            };

            //Preenche o parâmetros de competições atuais do DTO
            foreach (RunningCompetition runningCompetition in team.RunningCompetitions)
            {
                teamInfoDTO.NamesRunningCompetitions.Add(runningCompetition.Name);
            }

            return teamInfoDTO;
        }

        public static TeamMatchesDTO GetTeamMatches(TeamMatches teamMatches)
        {
            //Preenche o DTO de informações dos jogos do time
            var teamMatchesDTO = new TeamMatchesDTO
            {
                Competitions = teamMatches.ResultSet.Competitions,
                FirstMatch = teamMatches.ResultSet.First,
                LastMatch = teamMatches.ResultSet.Last,
                MatchesPlayed = teamMatches.ResultSet.Played,
                Wins = teamMatches.ResultSet.Wins,
                Draws = teamMatches.ResultSet.Draws,
                Losses = teamMatches.ResultSet.Losses,
                Matches = teamMatches.Matches?.Select(m => new MatchDTO
                {
                    Competition = new CompetitionDTO
                    {
                        CompetitionId = m.Competition.Id,
                        CompetitionName = m.Competition.Name,
                        CompetitionCode = m.Competition.Code,
                        StartDate = m.Season.StartDate,
                        EndDate = m.Season.EndDate,
                        Winner = m.Season.Winner != null ? new TeamDTO 
                        {
                            TeamId = m.Season.Winner.Id,
                            TeamName = m.Season.Winner.Name,
                            TeamShortName = m.Season.Winner.ShortName,
                            TeamTLA = m.Season.Winner.TLA
                        } : null
                    },
                    AreaName = m.Area.Name,
                    CurrentMatchday = m.Season.CurrentMatchday,
                    MatchDate = m.UtcDate,
                    Status = m.Status,
                    Matchday = m.Matchday,
                    Stage = m.Stage,
                    Group = m.Group,
                    HomeTeam = new TeamDTO
                    {
                        TeamId = m.HomeTeam.Id,
                        TeamName = m.HomeTeam.Name,
                        TeamShortName = m.HomeTeam.ShortName,
                        TeamTLA = m.HomeTeam.TLA,
                    },
                    AwayTeam = new TeamDTO
                    {
                        TeamId = m.AwayTeam.Id,
                        TeamName = m.AwayTeam.Name,
                        TeamShortName = m.AwayTeam.ShortName,
                        TeamTLA = m.AwayTeam.TLA,
                    },
                    Score = new ScoreDTO
                    {
                        Winner = m.Score.Winner,
                        Duration = m.Score.Duration,
                        FullTime = new TimeDTO
                        {
                            Home = m.Score.FullTime.Home,
                            Away = m.Score.FullTime.Away
                        },
                        HalfTime = new TimeDTO
                        {
                            Home = m.Score.HalfTime.Home,
                            Away = m.Score.HalfTime.Away
                        },
                    },
                    Referees = m.Referees?.Select(r => new RefereeDTO
                    {
                        Name = r.Name,
                        Type = r.Type,
                        Nationality = r.Nationality
                    })
                })
            };

            return teamMatchesDTO;
        }

        public static MatchDTO GetMatchInfo(Match match)
        {
            //Preenche o DTO de informações dos jogo
            var matchDTO = new MatchDTO
            {
                AreaName = match.Area.Name,
                Competition = new CompetitionDTO
                {
                    CompetitionId = match.Competition.Id,
                    CompetitionName = match.Competition.Name,
                    CompetitionCode = match.Competition.Code,
                    StartDate = match.Season.StartDate,
                    EndDate = match.Season.EndDate,
                    Winner = match.Season.Winner != null ? new TeamDTO
                    {
                        TeamId = match.Season.Winner.Id,
                        TeamName = match.Season.Winner.Name,
                        TeamShortName = match.Season.Winner.ShortName,
                        TeamTLA = match.Season.Winner.TLA
                    } : null
                },
                CurrentMatchday = match.Season.CurrentMatchday,
                MatchDate = match.UtcDate,
                Status = match.Status,
                Matchday = match.Matchday,
                Stage = match.Stage,
                Group = match.Group,
                HomeTeam = new TeamDTO
                {
                    TeamId = match.HomeTeam.Id,
                    TeamName = match.HomeTeam.Name,
                    TeamShortName = match.HomeTeam.ShortName,
                    TeamTLA = match.HomeTeam.TLA,
                },
                AwayTeam = new TeamDTO
                {
                    TeamId = match.AwayTeam.Id,
                    TeamName = match.AwayTeam.Name,
                    TeamShortName = match.AwayTeam.ShortName,
                    TeamTLA = match.AwayTeam.TLA,
                },
                Score = new ScoreDTO
                {
                    Winner = match.Score.Winner,
                    Duration = match.Score.Duration,
                    FullTime = new TimeDTO
                    {
                        Home = match.Score.FullTime.Home,
                        Away = match.Score.FullTime.Away
                    },
                    HalfTime = new TimeDTO
                    {
                        Home = match.Score.HalfTime.Home,
                        Away = match.Score.HalfTime.Away
                    },
                },
                Referees = match.Referees?.Select(r => new RefereeDTO
                {
                    Name = r.Name,
                    Type = r.Type,
                    Nationality = r.Nationality
                })
            };

            return matchDTO;
        }

        public static PlayerInfoDTO GetPlayerInfo(Player player)
        {
            //Preenche o DTO de informações mais detalhadas do jogador
            var playerInfoDTO = new PlayerInfoDTO
            {
                Id = player.Id,
                Name = player.Name,
                FirstName = player.FirstName,
                LastName = player.LastName,
                DateOfBirth = player.DateOfBirth,
                Age = player.CalcPlayerAge(),
                Nationality = player.Nationality,
                Section = player.Section,
                Position = player.Position,
                ShirtNumber = player.ShirtNumber,
                CurrentTeam = new TeamInfoDTO
                {
                    Nationality = player.CurrentTeam.Area.Name,
                    Name = player.CurrentTeam.Name,
                    ShortName = player.CurrentTeam.ShortName,
                    Founded = player.CurrentTeam.Founded.Value,
                    ClubColors = player.CurrentTeam.ClubColors,
                    Venue = player.CurrentTeam.Venue,
                },
                Contract = new Contract
                {
                    Start = player.CurrentTeam.Contract.Start,
                    Until = player.CurrentTeam.Contract.Until
                }
            };

            //Preenche o parâmetros de competições atuais do DTO
            foreach (RunningCompetition runningCompetition in player.CurrentTeam.RunningCompetitions)
            {
                playerInfoDTO.CurrentTeam.NamesRunningCompetitions.Add(runningCompetition.Name);
            }

            return playerInfoDTO;
        }

        public static PlayerMatchesDTO GetPlayerMatches(PlayerMatches playerMatches)
        {
            //Preenche o DTO de informações dos jogos do jogador
            var playerMatchesDTO = new PlayerMatchesDTO
            {
                Competitions = playerMatches.ResultSet.Competitions,
                FirstMatch = playerMatches.ResultSet.First,
                LastMatch = playerMatches.ResultSet.Last,
                Player = new PlayerInfoDTO
                {
                    Id = playerMatches.Person.Id,
                    Name = playerMatches.Person.Name,
                    FirstName = playerMatches.Person.FirstName,
                    LastName = playerMatches.Person.LastName,
                    DateOfBirth = playerMatches.Person.DateOfBirth,
                    Age = playerMatches.Person.CalcPlayerAge(),
                    Nationality = playerMatches.Person.Nationality,
                    Section = playerMatches.Person.Section,
                    Position = playerMatches.Person.Position,
                    ShirtNumber = playerMatches.Person.ShirtNumber
                },
                Matches = playerMatches.Matches?.Select(m => new MatchDTO
                {
                    Competition = new CompetitionDTO
                    {
                        CompetitionId = m.Competition.Id,
                        CompetitionName = m.Competition.Name,
                        CompetitionCode = m.Competition.Code,
                        StartDate = m.Season.StartDate,
                        EndDate = m.Season.EndDate,
                        Winner = m.Season.Winner != null ? new TeamDTO
                        {
                            TeamId = m.Season.Winner.Id,
                            TeamName = m.Season.Winner.Name,
                            TeamShortName = m.Season.Winner.ShortName,
                            TeamTLA = m.Season.Winner.TLA
                        } : null
                    },
                    AreaName = m.Area.Name,
                    CurrentMatchday = m.Season.CurrentMatchday,
                    MatchDate = m.UtcDate,
                    Status = m.Status,
                    Matchday = m.Matchday,
                    Stage = m.Stage,
                    Group = m.Group,
                    HomeTeam = new TeamDTO
                    {
                        TeamId = m.HomeTeam.Id,
                        TeamName = m.HomeTeam.Name,
                        TeamShortName = m.HomeTeam.ShortName,
                        TeamTLA = m.HomeTeam.TLA,
                    },
                    AwayTeam = new TeamDTO
                    {
                        TeamId = m.AwayTeam.Id,
                        TeamName = m.AwayTeam.Name,
                        TeamShortName = m.AwayTeam.ShortName,
                        TeamTLA = m.AwayTeam.TLA,
                    },
                    Score = new ScoreDTO
                    {
                        Winner = m.Score.Winner,
                        Duration = m.Score.Duration,
                        FullTime = new TimeDTO
                        {
                            Home = m.Score.FullTime.Home,
                            Away = m.Score.FullTime.Away
                        },
                        HalfTime = new TimeDTO
                        {
                            Home = m.Score.HalfTime.Home,
                            Away = m.Score.HalfTime.Away
                        },
                    },
                    Referees = m.Referees?.Select(r => new RefereeDTO
                    {
                        Name = r.Name,
                        Type = r.Type,
                        Nationality = r.Nationality
                    })
                })
            };

            return playerMatchesDTO;
        }

        public static CompetitionTableDTO GetCompetitionTable(CompetitionTable competitionTable)
        {
            //Preenche o DTO de informações da tabela de pontuação da competição
            var competitionTableDTO = new CompetitionTableDTO
            {
                AreaName = competitionTable.Area.Name,
                CompetitionName = competitionTable.Competition.Name,
                StartDate = competitionTable.Season.StartDate,
                EndDate = competitionTable.Season.EndDate,
                CurrentMatchday = competitionTable.Season.CurrentMatchday,
                Winner = competitionTable.Season.Winner != null ? new TeamDTO 
                {
                    TeamId = competitionTable.Season.Winner.Id,
                    TeamName = competitionTable.Season.Winner.Name,
                    TeamShortName = competitionTable.Season.Winner.ShortName,
                    TeamTLA = competitionTable.Season.Winner.TLA
                } : null,
                Standings = competitionTable.Standings?.Select(s => new StandingDTO
                {
                    Stage = s.Stage,
                    Type = s.Type,
                    Group = s.Group,
                    Table = s.Table?.Select(t => new PositionTableDTO
                    {
                        Position = t.Position,
                        Team = new TeamDTO
                        {
                            TeamId = t.Team.Id,
                            TeamName = t.Team.Name,
                            TeamShortName = t.Team.ShortName,
                            TeamTLA = t.Team.TLA
                        },
                        PlayedGames = t.PlayedGames,
                        Form = t.Form,
                        Won = t.Won,
                        Draw = t.Draw,
                        Lost = t.Lost,
                        Points = t.Points,
                        GoalsFor = t.GoalsFor,
                        GoalsAgainst = t.GoalsAgainst,
                        GoalDifference = t.GoalDifference
                    })
                })
            };

            return competitionTableDTO;
        }

        public static TopScorersDTO GetTopScorers(TopScorers topScorers)
        {
            //Preenche o DTO de informações de artilheiros da competição
            var topScorersDTO = new TopScorersDTO 
            {
                Competition = new CompetitionDTO
                {
                    CompetitionId = topScorers.Competition.Id,
                    CompetitionCode = topScorers.Competition.Code,
                    CompetitionName = topScorers.Competition.Name,
                    StartDate = topScorers.Season.StartDate,
                    EndDate = topScorers.Season.EndDate,
                    Winner = topScorers.Season.Winner != null ? new TeamDTO
                    {
                        TeamId = topScorers.Season.Winner.Id,
                        TeamName = topScorers.Season.Winner.Name,
                        TeamShortName = topScorers.Season.Winner.ShortName,
                        TeamTLA = topScorers.Season.Winner.TLA
                    } : null,
                },
                TypeCompetition = topScorers.Competition.Type,
                CurrentMatchday = topScorers.Season.CurrentMatchday,
                Scorers = topScorers.Scorers?.Select( s => new ScorerDTO
                {
                    Player = new PlayerDTO
                    {
                        Id = s.Player.Id,
                        Name = s.Player.Name,
                        Age = s.Player.CalcPlayerAge(),
                        Position = s.Player.Position,
                        Nationality = s.Player.Nationality
                    },
                    Team = new TeamDTO
                    {
                        TeamId = s.Team.Id,
                        TeamName = s.Team.Name,
                        TeamShortName = s.Team.ShortName,
                        TeamTLA = s.Team.TLA
                    },
                    PlayedMatches = s.PlayedMatches,
                    Goals = s.Goals,
                    Assists = s.Assists,
                    Penalties = s.Penalties
                })
            };
            return topScorersDTO;
        }
    }
}