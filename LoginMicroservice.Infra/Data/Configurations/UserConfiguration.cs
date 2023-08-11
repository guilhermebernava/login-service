﻿using LoginMicroservice.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LoginMicroservice.Infra.Data.Configurations;

public class UserConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
       builder.HasKey(x => x.Id);
       builder.Property(x => x.PasswordSalt).IsRequired();
       builder.Property(x => x.PasswordHash).IsRequired();
       builder.Property(x => x.Email).IsRequired();
    }
}
