﻿' <auto-generated />
Imports System
Imports GetStarted
Imports Microsoft.EntityFrameworkCore
Imports Microsoft.EntityFrameworkCore.Infrastructure
Imports Microsoft.EntityFrameworkCore.Metadata
Imports Microsoft.EntityFrameworkCore.Migrations

Namespace Global.GetStarted.Migrations
    <DbContext(GetType(BloggingContext))>
    Partial Class BloggingContextModelSnapshot
        Inherits ModelSnapshot

        Protected Overrides Sub BuildModel(modelBuilder As ModelBuilder)
            modelBuilder.
                HasAnnotation("ProductVersion", "5.0.6")

            modelBuilder.Entity("GetStarted.Blog",
                Sub(b)
                    b.Property(Of Integer)("BlogId").
                        ValueGeneratedOnAdd().
                        HasColumnType("INTEGER")

                    b.Property(Of String)("Url").
                        HasColumnType("TEXT")

                    b.HasKey("BlogId")

                    b.ToTable("Blogs")
                End Sub)

            modelBuilder.Entity("GetStarted.Post",
                Sub(b)
                    b.Property(Of Integer)("PostId").
                        ValueGeneratedOnAdd().
                        HasColumnType("INTEGER")

                    b.Property(Of Integer)("BlogId").
                        HasColumnType("INTEGER")

                    b.Property(Of String)("Content").
                        HasColumnType("TEXT")

                    b.Property(Of String)("Title").
                        HasColumnType("TEXT")

                    b.HasKey("PostId")

                    b.HasIndex("BlogId")

                    b.ToTable("Posts")
                End Sub)

            modelBuilder.Entity("GetStarted.Post",
                Sub(b)
                    b.HasOne("GetStarted.Blog", "Blog").
                        WithMany("Posts").
                        HasForeignKey("BlogId").
                        OnDelete(DeleteBehavior.Cascade).
                        IsRequired()
                    b.Navigation("Blog")
                End Sub)

            modelBuilder.Entity("GetStarted.Blog",
                Sub(b)
                    b.Navigation("Posts")
                End Sub)
        End Sub
    End Class
End Namespace
