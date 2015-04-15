using System;
using System.Linq;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using FoursquareOAuth.ApiClasses;
using System.Data.Entity.Validation;

namespace DatabaseContext
{
    public class CounterDbContext : DbContext
    {
        public DbSet<UserProfile> UserProfiles { get; set; }
        public DbSet<AccessToken> AccessTokens { get; set; }
        public DbSet<Venue> Venues { get; set; }
        public DbSet<RequestRecord> RequestLog { get; set; }
        public DbSet<PushCheckin> PushCheckins { get; set; }
        public DbSet<PreOrder> PreOrders { get; set; }
        public DbSet<Subscription> Subscriptions { get; set; }
        public DbSet<SubscriptionType> SubscriptionTypes { get; set; }

        public CounterDbContext()
            : base("DefaultConnection")
        {
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<UserProfile>()
                .HasMany(x => x.ManagedVenues)
                .WithMany(x => x.Managers)
                .Map(x =>
                {
                    x.ToTable("VenueManager");
                    x.MapLeftKey("UserId");
                    x.MapRightKey("VenueId");
                });

            modelBuilder.Entity<PushCheckin>()
                .HasRequired(c => c.Venue)
                .WithMany(v => v.PushCheckins);
        }

        protected override System.Data.Entity.Validation.DbEntityValidationResult ValidateEntity(System.Data.Entity.Infrastructure.DbEntityEntry entityEntry, IDictionary<object, object> items)
        {
            DbEntityValidationResult result = new DbEntityValidationResult(entityEntry, new List<DbValidationError>());

            //validate subscription type
            if(entityEntry.Entity is SubscriptionType)
            {
                SubscriptionType subtype = entityEntry.Entity as SubscriptionType;

                if (subtype.AdministratorOnly && !subtype.RequiresAccount)
                {
                    result.ValidationErrors.Add(new DbValidationError("RequiresAccount", "An account is required to enforce administrator-only access"));
                }
            }
            //validate subscription
            else if (entityEntry.Entity is Subscription)
            {
                Subscription sub = entityEntry.Entity as Subscription;

                //make sure an email or user was provided
                if (sub.Email == null && !sub.UserId.HasValue)
                {
                    result.ValidationErrors.Add(new DbValidationError("Email", "An email address or user profile is required."));
                    result.ValidationErrors.Add(new DbValidationError("UserId", "An email address or user profile is required."));
                }

                //make sure only one of email or user was provided
                if (sub.Email != null && sub.UserId.HasValue)
                {
                    result.ValidationErrors.Add(new DbValidationError("Email", "Only one of 'Email' or 'UserId' can be provided."));
                    result.ValidationErrors.Add(new DbValidationError("UserId", "Only one of 'Email' or 'UserId' can be provided."));
                }
            }

            if (result.ValidationErrors.Count > 0)
            {
                return result;
            }
            else
            {
                return base.ValidateEntity(entityEntry, items);
            }
        }
    }

    [Table("UserProfile")]
    public class UserProfile
    {
        [Key]
        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
        public int UserId { get; set; }
        public string AccessToken { get; set; } //deprecated
        public string Email { get; set; }
        public string UserName { get; set; }
        public string PhoneNumber { get; set; }
        public virtual ICollection<Venue> ManagedVenues { get; set; }
        public virtual ICollection<Subscription> Subscriptions { get; set; }

        [Required]
        [ForeignKey("AccessTokenRef")]
        public string FoursquareId { get; set; }
        public virtual AccessToken AccessTokenRef { get; set; }  //this should now be used to get the access token
    }

    [Table("AccessToken")]
    public class AccessToken
    {
        [Key]
        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.None)]
        public string FoursquareId { get; set; }

        public string Token { get; set; }
    }

    [Table("Venue")]
    public class Venue
    {
        [Key]
        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.None)]
        public string VenueId { get; set; }
        public string Name { get; set; }
        public int CheckinCount { get; set; }
        public DateTime lastUpdated { get; set; }
        public virtual ICollection<UserProfile> Managers { get; set; }
        public virtual ICollection<PushCheckin> PushCheckins { get; set; }
    }

    [Table("VenueManager")]
    public class VenueManager
    {
        [ForeignKey("Venue")]
        public string VenueId { get; set; }
        public virtual Venue Venue { get; set; }

        [ForeignKey("UserProfile")]
        public int UserId { get; set; }
        public virtual UserProfile UserProfile { get; set; }
    }

    [Table("Checkin")]
    public class PushCheckin
    {
        [Key]
        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.None)]
        public string Id { get; set; }
        public string UserId { get; set; }
        public string UserName { get; set; }
        public DateTime CheckinTime { get; set; }

        [ForeignKey("Venue")]
        public string VenueId { get; set; }
        public virtual Venue Venue { get; set; }

        public PushCheckin() { }

        public PushCheckin(FSqCheckin checkin)
        {
            this.Id = checkin.id;
            this.UserId = checkin.user.id;
            this.UserName = String.Format("{0} {1}", checkin.user.firstName, checkin.user.lastName).Trim();
            //TODO: extract created time from checkin.created and checkin.timeZoneOffset
            this.CheckinTime = DateTime.Now;
            this.VenueId = checkin.venue.id;
        }
    }

    [Table("PreOrder")]
    public class PreOrder
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        public string BusinessName { get; set; }

        [Required]
        public string Name { get; set; }

        public string EmailAddress { get; set; }
        public string PhoneNumber { get; set; }
        public string Address { get; set; }

        [MaxLength]
        public string Message { get; set; }

        public PreOrder() { }
    }

    [Table("SubscriptionType")]
    public class SubscriptionType
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public string Description { get; set; }

        [Required]
        public bool AdministratorOnly { get; set; }

        [Required]
        public bool RequiresAccount { get; set; }

        public virtual ICollection<Subscription> Subscriptions { get; set; }
    }

    [Table("Subscription")]
    public class Subscription
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [ForeignKey("SubscriptionType")]
        //TODO: check implications of this then uncomment
        //[Index("IX_TypeAndUser", Order = 1, IsUnique = true)]
        public int SubscriptionTypeId { get; set; }
        public virtual SubscriptionType SubscriptionType { get; set; }

        public string Email { get; set; }

        [ForeignKey("User")]
        //[Index("IX_TypeAndUser", Order = 2, IsUnique = true)]
        public int? UserId { get; set; }
        public virtual UserProfile User { get; set; }
    }

    [Table("RequestRecord")]
    public class RequestRecord
    {
        [Key]
        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
        public long ID { get; set; }
        public string Url { get; set; }
        public string RequestType { get; set; }
        public string RequestHeader { get; set; }
        public string RequestBody { get; set; }
        public string IPAddress { get; set; }
        public string Action { get; set; }
        public string Controller { get; set; }
        public DateTime RequestDate { get; set; }
    }
}