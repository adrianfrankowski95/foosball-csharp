﻿// <auto-generated />
using System;
using Foosball.CSharp.Infrastructure;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Foosball.CSharp.API.Migrations
{
    [DbContext(typeof(FoosballDbContext))]
    [Migration("20230329135809_AddedSetFinishedAt")]
    partial class AddedSetFinishedAt
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.4")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("Foosball.CSharp.Domain.GameAggregateModel.Game", b =>
                {
                    b.Property<Guid>("Id")
                        .HasColumnType("uuid")
                        .HasColumnName("id");

                    b.Property<DateTime>("StartedAt")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("started_at");

                    b.Property<string>("status")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("status");

                    b.HasKey("Id")
                        .HasName("pk_games");

                    b.HasIndex("StartedAt")
                        .IsDescending()
                        .HasDatabaseName("ix_games_started_at");

                    b.ToTable("games", (string)null);

                    b.HasDiscriminator<string>("status").HasValue("Game");

                    b.UseTphMappingStrategy();
                });

            modelBuilder.Entity("Foosball.CSharp.Domain.GameAggregateModel.Set", b =>
                {
                    b.Property<Guid>("Id")
                        .HasColumnType("uuid")
                        .HasColumnName("id");

                    b.Property<Guid>("GameId")
                        .HasColumnType("uuid")
                        .HasColumnName("game_id");

                    b.Property<Guid>("TeamAId")
                        .HasColumnType("uuid")
                        .HasColumnName("team_a_id");

                    b.Property<Guid>("TeamBId")
                        .HasColumnType("uuid")
                        .HasColumnName("team_b_id");

                    b.Property<string>("status")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("status");

                    b.HasKey("Id")
                        .HasName("pk_sets");

                    b.HasIndex("GameId")
                        .HasDatabaseName("ix_sets_game_id");

                    b.HasIndex("TeamAId")
                        .HasDatabaseName("ix_sets_team_a_id");

                    b.HasIndex("TeamBId")
                        .HasDatabaseName("ix_sets_team_b_id");

                    b.ToTable("sets", (string)null);

                    b.HasDiscriminator<string>("status").HasValue("Set");

                    b.UseTphMappingStrategy();
                });

            modelBuilder.Entity("Foosball.CSharp.Domain.TeamAggregateModel.Player", b =>
                {
                    b.Property<Guid>("Id")
                        .HasColumnType("uuid")
                        .HasColumnName("id");

                    b.Property<string>("FirstName")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("first_name");

                    b.Property<string>("LastName")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("last_name");

                    b.HasKey("Id")
                        .HasName("pk_players");

                    b.ToTable("players", (string)null);
                });

            modelBuilder.Entity("Foosball.CSharp.Domain.TeamAggregateModel.Team", b =>
                {
                    b.Property<Guid>("Id")
                        .HasColumnType("uuid")
                        .HasColumnName("id");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("name");

                    b.Property<string>("type")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("type");

                    b.HasKey("Id")
                        .HasName("pk_teams");

                    b.HasIndex("Name")
                        .IsUnique()
                        .HasDatabaseName("ix_teams_name");

                    b.ToTable("teams", (string)null);

                    b.HasDiscriminator<string>("type").HasValue("Team");

                    b.UseTphMappingStrategy();
                });

            modelBuilder.Entity("Foosball.CSharp.Domain.GameAggregateModel.FinishedGame", b =>
                {
                    b.HasBaseType("Foosball.CSharp.Domain.GameAggregateModel.Game");

                    b.Property<Guid>("WinnerTeamId")
                        .HasColumnType("uuid")
                        .HasColumnName("winner_team_id");

                    b.HasIndex("WinnerTeamId")
                        .HasDatabaseName("ix_games_winner_team_id");

                    b.HasDiscriminator().HasValue("finished");
                });

            modelBuilder.Entity("Foosball.CSharp.Domain.GameAggregateModel.GameInProgress", b =>
                {
                    b.HasBaseType("Foosball.CSharp.Domain.GameAggregateModel.Game");

                    b.HasDiscriminator().HasValue("in_progress");
                });

            modelBuilder.Entity("Foosball.CSharp.Domain.GameAggregateModel.FinishedSet", b =>
                {
                    b.HasBaseType("Foosball.CSharp.Domain.GameAggregateModel.Set");

                    b.Property<DateTime>("FinishedAt")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("finished_at");

                    b.Property<Guid>("WinnerTeamId")
                        .HasColumnType("uuid")
                        .HasColumnName("winner_team_id");

                    b.HasIndex("WinnerTeamId")
                        .HasDatabaseName("ix_sets_winner_team_id");

                    b.HasDiscriminator().HasValue("finished");
                });

            modelBuilder.Entity("Foosball.CSharp.Domain.GameAggregateModel.SetInProgress", b =>
                {
                    b.HasBaseType("Foosball.CSharp.Domain.GameAggregateModel.Set");

                    b.HasDiscriminator().HasValue("in_progress");
                });

            modelBuilder.Entity("Foosball.CSharp.Domain.TeamAggregateModel.OnePlayerTeam", b =>
                {
                    b.HasBaseType("Foosball.CSharp.Domain.TeamAggregateModel.Team");

                    b.Property<Guid>("PlayerId")
                        .ValueGeneratedOnUpdateSometimes()
                        .HasColumnType("uuid")
                        .HasColumnName("first_player_id");

                    b.HasIndex("PlayerId")
                        .HasDatabaseName("ix_teams_first_player_id");

                    b.HasDiscriminator().HasValue("one-player");
                });

            modelBuilder.Entity("Foosball.CSharp.Domain.TeamAggregateModel.TwoPlayersTeam", b =>
                {
                    b.HasBaseType("Foosball.CSharp.Domain.TeamAggregateModel.Team");

                    b.Property<Guid>("FirstPlayerId")
                        .ValueGeneratedOnUpdateSometimes()
                        .HasColumnType("uuid")
                        .HasColumnName("first_player_id");

                    b.Property<Guid>("SecondPlayerId")
                        .HasColumnType("uuid")
                        .HasColumnName("second_player_id");

                    b.HasIndex("FirstPlayerId")
                        .HasDatabaseName("ix_teams_first_player_id");

                    b.HasIndex("SecondPlayerId")
                        .HasDatabaseName("ix_teams_second_player_id");

                    b.HasDiscriminator().HasValue("two-players");
                });

            modelBuilder.Entity("Foosball.CSharp.Domain.GameAggregateModel.Set", b =>
                {
                    b.HasOne("Foosball.CSharp.Domain.GameAggregateModel.GameInProgress", null)
                        .WithMany("Sets")
                        .HasForeignKey("GameId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("fk_sets_games_game_id");

                    b.HasOne("Foosball.CSharp.Domain.TeamAggregateModel.Team", null)
                        .WithMany()
                        .HasForeignKey("TeamAId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired()
                        .HasConstraintName("fk_sets_teams_team_temp_id");

                    b.HasOne("Foosball.CSharp.Domain.TeamAggregateModel.Team", null)
                        .WithMany()
                        .HasForeignKey("TeamBId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired()
                        .HasConstraintName("fk_sets_teams_team_temp_id1");

                    b.OwnsOne("Foosball.CSharp.Domain.GameAggregateModel.Scores", "Scores", b1 =>
                        {
                            b1.Property<Guid>("SetId")
                                .HasColumnType("uuid")
                                .HasColumnName("id");

                            b1.Property<int>("TeamAScore")
                                .HasColumnType("integer")
                                .HasColumnName("team_a_score");

                            b1.Property<int>("TeamBScore")
                                .HasColumnType("integer")
                                .HasColumnName("team_b_score");

                            b1.HasKey("SetId");

                            b1.ToTable("sets");

                            b1.WithOwner()
                                .HasForeignKey("SetId")
                                .HasConstraintName("fk_sets_sets_id");
                        });

                    b.Navigation("Scores")
                        .IsRequired();
                });

            modelBuilder.Entity("Foosball.CSharp.Domain.GameAggregateModel.FinishedGame", b =>
                {
                    b.HasOne("Foosball.CSharp.Domain.TeamAggregateModel.Team", null)
                        .WithMany()
                        .HasForeignKey("WinnerTeamId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired()
                        .HasConstraintName("fk_games_teams_team_temp_id");
                });

            modelBuilder.Entity("Foosball.CSharp.Domain.GameAggregateModel.FinishedSet", b =>
                {
                    b.HasOne("Foosball.CSharp.Domain.GameAggregateModel.FinishedGame", null)
                        .WithMany("Sets")
                        .HasForeignKey("GameId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("fk_set_games_finished_game_id");

                    b.HasOne("Foosball.CSharp.Domain.TeamAggregateModel.Team", null)
                        .WithMany()
                        .HasForeignKey("WinnerTeamId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired()
                        .HasConstraintName("fk_sets_teams_team_temp_id2");
                });

            modelBuilder.Entity("Foosball.CSharp.Domain.TeamAggregateModel.OnePlayerTeam", b =>
                {
                    b.HasOne("Foosball.CSharp.Domain.TeamAggregateModel.Player", null)
                        .WithMany()
                        .HasForeignKey("PlayerId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired()
                        .HasConstraintName("fk_teams_players_first_player_id");
                });

            modelBuilder.Entity("Foosball.CSharp.Domain.TeamAggregateModel.TwoPlayersTeam", b =>
                {
                    b.HasOne("Foosball.CSharp.Domain.TeamAggregateModel.Player", null)
                        .WithMany()
                        .HasForeignKey("FirstPlayerId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("fk_teams_players_player_id");

                    b.HasOne("Foosball.CSharp.Domain.TeamAggregateModel.Player", null)
                        .WithMany()
                        .HasForeignKey("SecondPlayerId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("fk_teams_players_player_id1");
                });

            modelBuilder.Entity("Foosball.CSharp.Domain.GameAggregateModel.FinishedGame", b =>
                {
                    b.Navigation("Sets");
                });

            modelBuilder.Entity("Foosball.CSharp.Domain.GameAggregateModel.GameInProgress", b =>
                {
                    b.Navigation("Sets");
                });
#pragma warning restore 612, 618
        }
    }
}
