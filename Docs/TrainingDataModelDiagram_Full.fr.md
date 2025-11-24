# Modèle de Données — Description Technique Complète

Ce document détaille les entités et les relations du modèle de données issu du diagramme PlantUML `TrainingDataModelDiagram_Full.puml`.

---

## Entités principales

### User (Utilisateur)
- **Id** : int (clé primaire)
- Name : string
- Email : string
- Role : string
- CreatedAt : DateTime
- IsTrainer : bool
- IsAdminUser : bool

### BillingInvoice (Facture)
- **Id** : int (clé primaire)
- Amount : decimal
- Date : DateTime
- CustomerId : int (clé étrangère)
- Status : string
- PaidDate : DateTime

### Customer (Client)
- **Id** : int (clé primaire)
- Name : string
- Email : string
- Address : string
- PostalCode : string
- City : string
- Country : string
- Phone : string
- LogoImagePath : string

### DailyUsageStatistic (Statistique d’Usage Quotidien)
- **Id** : int (clé primaire)
- Date : DateTime
- Usage : decimal
- UserId : int (clé étrangère)
- RegisteredModuleId : int (clé étrangère)

### RegisteredCourse (Cours Inscrit)
- **Id** : int (clé primaire)
- Title : string
- Description : string
- StartDate : DateTime
- EndDate : DateTime
- CustomerId : int (clé étrangère)
- TrainerId : int (clé étrangère vers User)

### RegisteredModule (Module Inscrit)
- **Id** : int (clé primaire)
- Title : string
- Description : string
- StartDate : DateTime
- EndDate : DateTime
- RegisteredCourseId : int (clé étrangère)
- TrainerId : int (clé étrangère vers User)

### StudentEnrollment (Inscription Étudiant)
- **Id** : int (clé primaire)
- **UserId** : int (clé étrangère)
- **RegisteredCourseId** : int (clé étrangère)
- **VirtualMachineId** : int (clé étrangère)

### RDPFile
- **Id** : int (clé primaire)
- Path : string
- CreatedAt : DateTime
- **VirtualMachineId** : int (clé étrangère)

### SSHFile
- **Id** : int (clé primaire)
- Path : string
- CreatedAt : DateTime
- **VirtualMachineId** : int (clé étrangère)

### VirtualMachine (Machine Virtuelle)
- **Id** : int (clé primaire)
- Name : string
- VMImageReferenceId : int (clé étrangère)
- VmTypeId : int (clé étrangère)
- VMUsagePropertiesId : int (clé étrangère)
- VMPropertiesId : int (clé étrangère)
- RDPFileId : int (clé étrangère)
- SSHFileId : int (clé étrangère)
- UserId : int (clé étrangère)
- CreatedAt : DateTime

### VMImageReference (Référence Image VM)
- **Id** : int (clé primaire)
- Name : string
- Publisher : string
- Offer : string
- Sku : string
- Version : string
- CreatedAt : DateTime

### VMType (Type de VM)
- **Id** : int (clé primaire)
- Name : string
- AzureName : string
- CpuCores : int
- MemoryInGB : int
- CreatedAt : DateTime

### VMUsageProperties (Propriétés d’Usage VM)
- **Id** : int (clé primaire)
- Name : string
- PreferedStartTime : Time
- PreferedEndTime : Time
- PreferedDailyHourUsage : int
- CreatedAt : DateTime

### VMProperties (Propriétés VM)
- **Id** : int (clé primaire)
- Name : string
- Description : string
- IsWindows11 : bool
- IsOffice2021 : bool
- IsTerraformInstalled : bool
- IsPhotoshopInstalled : bool
- CreatedAt : DateTime

---

## Relations entre entités

- **Customer** 1—* BillingInvoice : Un client possède plusieurs factures.
- **Customer** 1—* User : Un client possède plusieurs utilisateurs.
- **User** 1—* DailyUsageStatistic : Un utilisateur possède plusieurs statistiques d’usage.
- **User** 1—* StudentEnrollment : Un utilisateur peut avoir plusieurs inscriptions.
- **VMType** 1—* VirtualMachine : Un type de VM est utilisé par plusieurs machines virtuelles.
- **VMImageReference** 1—* VirtualMachine : Une image VM peut être utilisée par plusieurs machines virtuelles.
- **VMUsageProperties** 1—* VirtualMachine : Un profil d’usage peut être utilisé par plusieurs machines virtuelles.
- **VMProperties** 1—* VirtualMachine : Un ensemble de propriétés peut être utilisé par plusieurs machines virtuelles.
- **SSHFile** — VirtualMachine : Un fichier SSH peut être assigné à une machine virtuelle.
- **RDPFile** — VirtualMachine : Un fichier RDP peut être assigné à une machine virtuelle.
- **VirtualMachine** — StudentEnrollment : Une machine virtuelle est assignée à une inscription d’étudiant.
- **RegisteredModule** 1—* DailyUsageStatistic : Un module inscrit possède plusieurs statistiques d’usage.
- **RegisteredCourse** — User : Un cours inscrit référence un formateur (User).
- **RegisteredCourse** 1—* StudentEnrollment : Un cours inscrit possède plusieurs inscriptions d’étudiants.
- **RegisteredCourse** 1—* RegisteredModule : Un cours inscrit possède plusieurs modules inscrits.

---

**Remarques**
- Les clés primaires sont indiquées par `*`.
- Les clés étrangères sont précisées dans la description des champs.
- Les relations de type "1 à plusieurs" sont notées `1—*`.
- Les relations d’affectation ou de lien sont précisées dans la section relations.

---

*Document généré automatiquement à partir du diagramme PlantUML.*
