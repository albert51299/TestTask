using Microsoft.EntityFrameworkCore;

namespace TestTask.Models {
    public class VaccinationsContext : DbContext {
        public DbSet<Patient> Patients { get; set; }
        public DbSet<Vaccination> Vaccinations { get; set; }
        public DbSet<Vaccine> Vaccines { get; set; }

        public VaccinationsContext(DbContextOptions options) : base(options) {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder) {
            modelBuilder.Entity<Patient>().HasData(
                    new Patient {
                        Id = 1,
                        FirstName = "Иван",
                        LastName = "Васильев",
                        DateOfBirth = new System.DateTime(1980, 1, 2),
                        Gender = "м",
                        SNILS = "112-233-445 95"
                    },
                    new Patient {
                        Id = 2,
                        FirstName = "Василий",
                        LastName = "Иванов",
                        DateOfBirth = new System.DateTime(1990, 3, 4),
                        Gender = "м",
                        SNILS = "112-233-446 96"
                    },
                    new Patient {
                        Id = 3,
                        FirstName = "Алексей",
                        LastName = "Смирнов",
                        DateOfBirth = new System.DateTime(1995, 7, 26),
                        Gender = "м",
                        SNILS = "112-233-436 94"
                    },
                    new Patient {
                        Id = 4,
                        FirstName = "Петр",
                        LastName = "Иванов",
                        DateOfBirth = new System.DateTime(1965, 9, 15),
                        Gender = "м",
                        SNILS = "111-233-446 89"
                    },
                    new Patient {
                        Id = 5,
                        FirstName = "Валентина",
                        LastName = "Иванова",
                        DateOfBirth = new System.DateTime(1985, 11, 18),
                        Gender = "ж",
                        SNILS = "111-133-446 83"
                    }
                );

            modelBuilder.Entity<Vaccine>().HasData(
                    new Vaccine {
                        Id = 1,
                        Name = "Эджерикс"
                    },
                    new Vaccine {
                        Id = 2,
                        Name = "Вианвак"
                    },
                    new Vaccine {
                        Id = 3,
                        Name = "АКДС"
                    },
                    new Vaccine {
                        Id = 4,
                        Name = "БЦЖ"
                    }
                );

            modelBuilder.Entity<Vaccination>().HasData(
                    new Vaccination {
                        Id = 1,
                        VaccineName = "Эджерикс",
                        Consent = false,
                        Date = new System.DateTime(2019, 2, 1),
                        PatientId = 1
                    },
                    new Vaccination {
                        Id = 2,
                        VaccineName = "Вианвак",
                        Consent = false,
                        Date = new System.DateTime(2019, 2, 2),
                        PatientId = 1
                    },
                    new Vaccination {
                        Id = 3,
                        VaccineName = "АКДС",
                        Consent = true,
                        Date = new System.DateTime(2019, 3, 1),
                        PatientId = 2
                    },
                    new Vaccination {
                        Id = 4,
                        VaccineName = "БЦЖ",
                        Consent = true,
                        Date = new System.DateTime(2019, 3, 2),
                        PatientId = 2
                    }
                );
        }
    }
}
