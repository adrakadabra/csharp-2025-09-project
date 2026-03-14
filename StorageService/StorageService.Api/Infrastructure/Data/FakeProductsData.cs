using StorageService.Api.Domain.Entities;

namespace StorageService.Api.Infrastructure.Data;

///// <summary>
///// Демо-данные для тестирования
///// </summary>
//public static class FakeProductsData
//{
//    private static readonly Guid AutoSectionId = Guid.Parse("10000000-0000-0000-0000-000000000001");
//    private static readonly Guid DairySectionId = Guid.Parse("10000000-0000-0000-0000-000000000002");
//    private static readonly Guid BakerySectionId = Guid.Parse("10000000-0000-0000-0000-000000000003");
//    private static readonly Guid FruitsSectionId = Guid.Parse("10000000-0000-0000-0000-000000000004");
//    private static readonly Guid VegetablesSectionId = Guid.Parse("10000000-0000-0000-0000-000000000005");
//    private static readonly Guid HouseholdSectionId = Guid.Parse("10000000-0000-0000-0000-000000000006");
//    private static readonly Guid BeautySectionId = Guid.Parse("10000000-0000-0000-0000-000000000007");
//    private static readonly Guid ElectronicsSectionId = Guid.Parse("10000000-0000-0000-0000-000000000008");
//    private static readonly Guid TiresSectionId = Guid.Parse("10000000-0000-0000-0000-000000000009");
//    private static readonly Guid MeatSectionId = Guid.Parse("10000000-0000-0000-0000-000000000010");
//    private static readonly Guid DrinksSectionId = Guid.Parse("10000000-0000-0000-0000-000000000011");
//    private static readonly Guid BabySectionId = Guid.Parse("10000000-0000-0000-0000-000000000012");

//    public static List<Section> Sections { get; } = new()
//    {
//        new Section
//        {
//            Id = AutoSectionId,
//            Code = "AUTO",
//            Description = "Автотовары"
//        },
//        new Section
//        {
//            Id = DairySectionId,
//            Code = "DAIRY",
//            Description = "Молочные продукты"
//        },
//        new Section
//        {
//            Id = BakerySectionId,
//            Code = "BAKERY",
//            Description = "Хлеб и выпечка"
//        },
//        new Section
//        {
//            Id = FruitsSectionId,
//            Code = "FRUITS",
//            Description = "Фрукты"
//        },
//        new Section
//        {
//            Id = VegetablesSectionId,
//            Code = "VEGET",
//            Description = "Овощи"
//        },
//        new Section
//        {
//            Id = HouseholdSectionId,
//            Code = "HOME",
//            Description = "Товары для дома"
//        },
//        new Section
//        {
//            Id = BeautySectionId,
//            Code = "BEAUTY",
//            Description = "Красота и уход"
//        },
//        new Section
//        {
//            Id = ElectronicsSectionId,
//            Code = "ELECTRO",
//            Description = "Электроника"
//        },
//        new Section
//        {
//            Id = TiresSectionId,
//            Code = "TIRES",
//            Description = "Шины"
//        },
//        new Section
//        {
//            Id = MeatSectionId,
//            Code = "MEAT",
//            Description = "Мясо и птица"
//        },
//        new Section
//        {
//            Id = DrinksSectionId,
//            Code = "DRINKS",
//            Description = "Напитки"
//        },
//        new Section
//        {
//            Id = BabySectionId,
//            Code = "BABY",
//            Description = "Детские товары"
//        }
//    };

//    /// <summary>
//    /// Продукты
//    /// </summary>
//    public static List<Product> Products { get; } = new()
//    {
//        new Product
//        {
//            Id = Guid.Parse("20000000-0000-0000-0000-000000000001"),
//            CreatedAt = DateTime.UtcNow,
//            CreatedBy = "Me",
//            Article = "AUTO-0001",
//            Description = "Масло моторное синтетическое 5W-40, 4 л",
//            IsDeleted = false,
//            Name = "Машинное масло",
//            Quantity = 12,
//            Price = 1250,
//            Category = new Category
//            {
//                Id = Guid.Parse("30000000-0000-0000-0000-000000000001"),
//                Name = "Автомобильные товары",
//                Description = "Автомобильные товары"
//            },
//            Manufacturer = new Manufacturer
//            {
//                Id = Guid.Parse("40000000-0000-0000-0000-000000000001"),
//                Name = "Shell",
//                Country = "Германия"
//            },
//            SectionId = AutoSectionId
//        },
//        new Product
//        {
//            Id = Guid.Parse("20000000-0000-0000-0000-000000000002"),
//            CreatedAt = DateTime.UtcNow,
//            CreatedBy = "Me",
//            Article = "AUTO-0002",
//            Description = "Омыватель стекла зимний, -20°C, 4 л",
//            IsDeleted = false,
//            Name = "Омыватель стекла",
//            Quantity = 18,
//            Price = 349,
//            Category = new Category
//            {
//                Id = Guid.Parse("30000000-0000-0000-0000-000000000002"),
//                Name = "Автохимия",
//                Description = "Автохимия"
//            },
//            Manufacturer = new Manufacturer
//            {
//                Id = Guid.Parse("40000000-0000-0000-0000-000000000002"),
//                Name = "Liqui Moly",
//                Country = "Германия"
//            },
//            SectionId = AutoSectionId
//        },
//        new Product
//        {
//            Id = Guid.Parse("20000000-0000-0000-0000-000000000003"),
//            CreatedAt = DateTime.UtcNow,
//            CreatedBy = "Me",
//            Article = "DAIRY-0001",
//            Description = "Молоко ультрапастеризованное 3.2%, 1 л",
//            IsDeleted = false,
//            Name = "Молоко",
//            Quantity = 40,
//            Price = 119,
//            Category = new Category
//            {
//                Id = Guid.Parse("30000000-0000-0000-0000-000000000003"),
//                Name = "Молочные продукты",
//                Description = "Молочные продукты"
//            },
//            Manufacturer = new Manufacturer
//            {
//                Id = Guid.Parse("40000000-0000-0000-0000-000000000003"),
//                Name = "Домик в деревне",
//                Country = "Россия"
//            },
//            SectionId = DairySectionId
//        },
//        new Product
//        {
//            Id = Guid.Parse("20000000-0000-0000-0000-000000000004"),
//            CreatedAt = DateTime.UtcNow,
//            CreatedBy = "Me",
//            Article = "DAIRY-0002",
//            Description = "Сыр полутвёрдый, фасовка 200 г",
//            IsDeleted = false,
//            Name = "Сыр",
//            Quantity = 28,
//            Price = 219,
//            Category = new Category
//            {
//                Id = Guid.Parse("30000000-0000-0000-0000-000000000004"),
//                Name = "Молочные продукты",
//                Description = "Молочные продукты"
//            },
//            Manufacturer = new Manufacturer
//            {
//                Id = Guid.Parse("40000000-0000-0000-0000-000000000004"),
//                Name = "President",
//                Country = "Франция"
//            },
//            SectionId = DairySectionId
//        },
//        new Product
//        {
//            Id = Guid.Parse("20000000-0000-0000-0000-000000000005"),
//            CreatedAt = DateTime.UtcNow,
//            CreatedBy = "Me",
//            Article = "DAIRY-0003",
//            Description = "Йогурт клубничный, 270 г",
//            IsDeleted = false,
//            Name = "Йогурт",
//            Quantity = 35,
//            Price = 89,
//            Category = new Category
//            {
//                Id = Guid.Parse("30000000-0000-0000-0000-000000000005"),
//                Name = "Молочные продукты",
//                Description = "Молочные продукты"
//            },
//            Manufacturer = new Manufacturer
//            {
//                Id = Guid.Parse("40000000-0000-0000-0000-000000000005"),
//                Name = "Danone",
//                Country = "Россия"
//            },
//            SectionId = DairySectionId
//        },
//        new Product
//        {
//            Id = Guid.Parse("20000000-0000-0000-0000-000000000006"),
//            CreatedAt = DateTime.UtcNow,
//            CreatedBy = "Me",
//            Article = "BAKERY-0001",
//            Description = "Хлеб пшеничный нарезной, батон 400 г",
//            IsDeleted = false,
//            Name = "Хлеб белый",
//            Quantity = 30,
//            Price = 54,
//            Category = new Category
//            {
//                Id = Guid.Parse("30000000-0000-0000-0000-000000000006"),
//                Name = "Хлеб и выпечка",
//                Description = "Хлеб и выпечка"
//            },
//            Manufacturer = new Manufacturer
//            {
//                Id = Guid.Parse("40000000-0000-0000-0000-000000000006"),
//                Name = "Хлебозавод №1",
//                Country = "Россия"
//            },
//            SectionId = BakerySectionId
//        },
//        new Product
//        {
//            Id = Guid.Parse("20000000-0000-0000-0000-000000000007"),
//            CreatedAt = DateTime.UtcNow,
//            CreatedBy = "Me",
//            Article = "BAKERY-0002",
//            Description = "Булочка с маком, 100 г",
//            IsDeleted = false,
//            Name = "Булочка с маком",
//            Quantity = 25,
//            Price = 39,
//            Category = new Category
//            {
//                Id = Guid.Parse("30000000-0000-0000-0000-000000000007"),
//                Name = "Хлеб и выпечка",
//                Description = "Хлеб и выпечка"
//            },
//            Manufacturer = new Manufacturer
//            {
//                Id = Guid.Parse("40000000-0000-0000-0000-000000000007"),
//                Name = "Пекарня у дома",
//                Country = "Россия"
//            },
//            SectionId = BakerySectionId
//        },
//        new Product
//        {
//            Id = Guid.Parse("20000000-0000-0000-0000-000000000008"),
//            CreatedAt = DateTime.UtcNow,
//            CreatedBy = "Me",
//            Article = "FRUITS-0001",
//            Description = "Яблоки красные, фасовка 1 кг",
//            IsDeleted = false,
//            Name = "Яблоки",
//            Quantity = 50,
//            Price = 149,
//            Category = new Category
//            {
//                Id = Guid.Parse("30000000-0000-0000-0000-000000000008"),
//                Name = "Фрукты",
//                Description = "Фрукты"
//            },
//            Manufacturer = new Manufacturer
//            {
//                Id = Guid.Parse("40000000-0000-0000-0000-000000000008"),
//                Name = "FreshFarm",
//                Country = "Сербия"
//            },
//            SectionId = FruitsSectionId
//        },
//        new Product
//        {
//            Id = Guid.Parse("20000000-0000-0000-0000-000000000009"),
//            CreatedAt = DateTime.UtcNow,
//            CreatedBy = "Me",
//            Article = "FRUITS-0002",
//            Description = "Бананы, 1 кг",
//            IsDeleted = false,
//            Name = "Бананы",
//            Quantity = 45,
//            Price = 139,
//            Category = new Category
//            {
//                Id = Guid.Parse("30000000-0000-0000-0000-000000000009"),
//                Name = "Фрукты",
//                Description = "Фрукты"
//            },
//            Manufacturer = new Manufacturer
//            {
//                Id = Guid.Parse("40000000-0000-0000-0000-000000000009"),
//                Name = "Tropical Export",
//                Country = "Эквадор"
//            },
//            SectionId = FruitsSectionId
//        },
//        new Product
//        {
//            Id = Guid.Parse("20000000-0000-0000-0000-000000000010"),
//            CreatedAt = DateTime.UtcNow,
//            CreatedBy = "Me",
//            Article = "VEGET-0001",
//            Description = "Картофель мытый, 1 кг",
//            IsDeleted = false,
//            Name = "Картофель",
//            Quantity = 60,
//            Price = 79,
//            Category = new Category
//            {
//                Id = Guid.Parse("30000000-0000-0000-0000-000000000010"),
//                Name = "Овощи",
//                Description = "Овощи"
//            },
//            Manufacturer = new Manufacturer
//            {
//                Id = Guid.Parse("40000000-0000-0000-0000-000000000010"),
//                Name = "Фермерское хозяйство",
//                Country = "Россия"
//            },
//            SectionId = VegetablesSectionId
//        },
//        new Product
//        {
//            Id = Guid.Parse("20000000-0000-0000-0000-000000000011"),
//            CreatedAt = DateTime.UtcNow,
//            CreatedBy = "Me",
//            Article = "VEGET-0002",
//            Description = "Огурцы короткоплодные, 600 г",
//            IsDeleted = false,
//            Name = "Огурцы",
//            Quantity = 26,
//            Price = 159,
//            Category = new Category
//            {
//                Id = Guid.Parse("30000000-0000-0000-0000-000000000011"),
//                Name = "Овощи",
//                Description = "Овощи"
//            },
//            Manufacturer = new Manufacturer
//            {
//                Id = Guid.Parse("40000000-0000-0000-0000-000000000011"),
//                Name = "Green Line",
//                Country = "Россия"
//            },
//            SectionId = VegetablesSectionId
//        },
//        new Product
//        {
//            Id = Guid.Parse("20000000-0000-0000-0000-000000000012"),
//            CreatedAt = DateTime.UtcNow,
//            CreatedBy = "Me",
//            Article = "HOME-0001",
//            Description = "Гель для стирки цветного белья, 1.95 л",
//            IsDeleted = false,
//            Name = "Гель для стирки",
//            Quantity = 20,
//            Price = 599,
//            Category = new Category
//            {
//                Id = Guid.Parse("30000000-0000-0000-0000-000000000012"),
//                Name = "Бытовая химия",
//                Description = "Бытовая химия"
//            },
//            Manufacturer = new Manufacturer
//            {
//                Id = Guid.Parse("40000000-0000-0000-0000-000000000012"),
//                Name = "Ariel",
//                Country = "Франция"
//            },
//            SectionId = HouseholdSectionId
//        },
//        new Product
//        {
//            Id = Guid.Parse("20000000-0000-0000-0000-000000000013"),
//            CreatedAt = DateTime.UtcNow,
//            CreatedBy = "Me",
//            Article = "HOME-0002",
//            Description = "Средство для мытья посуды, 900 мл",
//            IsDeleted = false,
//            Name = "Средство для посуды",
//            Quantity = 18,
//            Price = 249,
//            Category = new Category
//            {
//                Id = Guid.Parse("30000000-0000-0000-0000-000000000013"),
//                Name = "Бытовая химия",
//                Description = "Бытовая химия"
//            },
//            Manufacturer = new Manufacturer
//            {
//                Id = Guid.Parse("40000000-0000-0000-0000-000000000013"),
//                Name = "Fairy",
//                Country = "Чехия"
//            },
//            SectionId = HouseholdSectionId
//        },
//        new Product
//        {
//            Id = Guid.Parse("20000000-0000-0000-0000-000000000014"),
//            CreatedAt = DateTime.UtcNow,
//            CreatedBy = "Me",
//            Article = "HOME-0003",
//            Description = "Бумажные полотенца, 2 рулона",
//            IsDeleted = false,
//            Name = "Бумажные полотенца",
//            Quantity = 32,
//            Price = 129,
//            Category = new Category
//            {
//                Id = Guid.Parse("30000000-0000-0000-0000-000000000014"),
//                Name = "Товары для дома",
//                Description = "Товары для дома"
//            },
//            Manufacturer = new Manufacturer
//            {
//                Id = Guid.Parse("40000000-0000-0000-0000-000000000014"),
//                Name = "Zewa",
//                Country = "Германия"
//            },
//            SectionId = HouseholdSectionId
//        },
//        new Product
//        {
//            Id = Guid.Parse("20000000-0000-0000-0000-000000000015"),
//            CreatedAt = DateTime.UtcNow,
//            CreatedBy = "Me",
//            Article = "BEAUTY-0001",
//            Description = "Парфюмерная вода, женская, 50 мл",
//            IsDeleted = false,
//            Name = "Духи",
//            Quantity = 10,
//            Price = 3499,
//            Category = new Category
//            {
//                Id = Guid.Parse("30000000-0000-0000-0000-000000000015"),
//                Name = "Парфюмерия",
//                Description = "Парфюмерия"
//            },
//            Manufacturer = new Manufacturer
//            {
//                Id = Guid.Parse("40000000-0000-0000-0000-000000000015"),
//                Name = "Versal Beauty",
//                Country = "Франция"
//            },
//            SectionId = BeautySectionId
//        },
//        new Product
//        {
//            Id = Guid.Parse("20000000-0000-0000-0000-000000000016"),
//            CreatedAt = DateTime.UtcNow,
//            CreatedBy = "Me",
//            Article = "BEAUTY-0002",
//            Description = "Шампунь для ежедневного ухода, 400 мл",
//            IsDeleted = false,
//            Name = "Шампунь",
//            Quantity = 22,
//            Price = 389,
//            Category = new Category
//            {
//                Id = Guid.Parse("30000000-0000-0000-0000-000000000016"),
//                Name = "Уход за волосами",
//                Description = "Уход за волосами"
//            },
//            Manufacturer = new Manufacturer
//            {
//                Id = Guid.Parse("40000000-0000-0000-0000-000000000016"),
//                Name = "Elseve",
//                Country = "Франция"
//            },
//            SectionId = BeautySectionId
//        },
//        new Product
//        {
//            Id = Guid.Parse("20000000-0000-0000-0000-000000000017"),
//            CreatedAt = DateTime.UtcNow,
//            CreatedBy = "Me",
//            Article = "ELECTRO-0001",
//            Description = "Беспроводные наушники с зарядным кейсом",
//            IsDeleted = false,
//            Name = "Наушники беспроводные",
//            Quantity = 8,
//            Price = 2999,
//            Category = new Category
//            {
//                Id = Guid.Parse("30000000-0000-0000-0000-000000000017"),
//                Name = "Электроника",
//                Description = "Электроника"
//            },
//            Manufacturer = new Manufacturer
//            {
//                Id = Guid.Parse("40000000-0000-0000-0000-000000000017"),
//                Name = "Xiaomi",
//                Country = "Китай"
//            },
//            SectionId = ElectronicsSectionId
//        },
//        new Product
//        {
//            Id = Guid.Parse("20000000-0000-0000-0000-000000000018"),
//            CreatedAt = DateTime.UtcNow,
//            CreatedBy = "Me",
//            Article = "ELECTRO-0002",
//            Description = "Кабель USB-C, 1 метр",
//            IsDeleted = false,
//            Name = "Кабель USB-C",
//            Quantity = 35,
//            Price = 299,
//            Category = new Category
//            {
//                Id = Guid.Parse("30000000-0000-0000-0000-000000000018"),
//                Name = "Аксессуары",
//                Description = "Аксессуары для техники"
//            },
//            Manufacturer = new Manufacturer
//            {
//                Id = Guid.Parse("40000000-0000-0000-0000-000000000018"),
//                Name = "Baseus",
//                Country = "Китай"
//            },
//            SectionId = ElectronicsSectionId
//        },
//        new Product
//        {
//            Id = Guid.Parse("20000000-0000-0000-0000-000000000019"),
//            CreatedAt = DateTime.UtcNow,
//            CreatedBy = "Me",
//            Article = "TIRES-0001",
//            Description = "Летняя шина 205/55 R16",
//            IsDeleted = false,
//            Name = "Шины летние",
//            Quantity = 16,
//            Price = 6590,
//            Category = new Category
//            {
//                Id = Guid.Parse("30000000-0000-0000-0000-000000000019"),
//                Name = "Шины",
//                Description = "Автомобильные шины"
//            },
//            Manufacturer = new Manufacturer
//            {
//                Id = Guid.Parse("40000000-0000-0000-0000-000000000019"),
//                Name = "Michelin",
//                Country = "Франция"
//            },
//            SectionId = TiresSectionId
//        },
//        new Product
//        {
//            Id = Guid.Parse("20000000-0000-0000-0000-000000000020"),
//            CreatedAt = DateTime.UtcNow,
//            CreatedBy = "Me",
//            Article = "MEAT-0001",
//            Description = "Филе куриное охлаждённое, 1 кг",
//            IsDeleted = false,
//            Name = "Куриное филе",
//            Quantity = 24,
//            Price = 329,
//            Category = new Category
//            {
//                Id = Guid.Parse("30000000-0000-0000-0000-000000000020"),
//                Name = "Мясо и птица",
//                Description = "Мясо и птица"
//            },
//            Manufacturer = new Manufacturer
//            {
//                Id = Guid.Parse("40000000-0000-0000-0000-000000000020"),
//                Name = "Петелинка",
//                Country = "Россия"
//            },
//            SectionId = MeatSectionId
//        },
//        new Product
//        {
//            Id = Guid.Parse("20000000-0000-0000-0000-000000000021"),
//            CreatedAt = DateTime.UtcNow,
//            CreatedBy = "Me",
//            Article = "MEAT-0002",
//            Description = "Фарш говяжий охлаждённый, 500 г",
//            IsDeleted = false,
//            Name = "Фарш говяжий",
//            Quantity = 14,
//            Price = 289,
//            Category = new Category
//            {
//                Id = Guid.Parse("30000000-0000-0000-0000-000000000021"),
//                Name = "Мясо и птица",
//                Description = "Мясо и птица"
//            },
//            Manufacturer = new Manufacturer
//            {
//                Id = Guid.Parse("40000000-0000-0000-0000-000000000021"),
//                Name = "Мираторг",
//                Country = "Россия"
//            },
//            SectionId = MeatSectionId
//        },
//        new Product
//        {
//            Id = Guid.Parse("20000000-0000-0000-0000-000000000022"),
//            CreatedAt = DateTime.UtcNow,
//            CreatedBy = "Me",
//            Article = "DRINKS-0001",
//            Description = "Минеральная вода без газа, 1.5 л",
//            IsDeleted = false,
//            Name = "Минеральная вода",
//            Quantity = 48,
//            Price = 69,
//            Category = new Category
//            {
//                Id = Guid.Parse("30000000-0000-0000-0000-000000000022"),
//                Name = "Напитки",
//                Description = "Напитки"
//            },
//            Manufacturer = new Manufacturer
//            {
//                Id = Guid.Parse("40000000-0000-0000-0000-000000000022"),
//                Name = "Святой Источник",
//                Country = "Россия"
//            },
//            SectionId = DrinksSectionId
//        },
//        new Product
//        {
//            Id = Guid.Parse("20000000-0000-0000-0000-000000000023"),
//            CreatedAt = DateTime.UtcNow,
//            CreatedBy = "Me",
//            Article = "DRINKS-0002",
//            Description = "Сок апельсиновый, 1 л",
//            IsDeleted = false,
//            Name = "Апельсиновый сок",
//            Quantity = 27,
//            Price = 149,
//            Category = new Category
//            {
//                Id = Guid.Parse("30000000-0000-0000-0000-000000000023"),
//                Name = "Напитки",
//                Description = "Напитки"
//            },
//            Manufacturer = new Manufacturer
//            {
//                Id = Guid.Parse("40000000-0000-0000-0000-000000000023"),
//                Name = "J7",
//                Country = "Россия"
//            },
//            SectionId = DrinksSectionId
//        },
//        new Product
//        {
//            Id = Guid.Parse("20000000-0000-0000-0000-000000000024"),
//            CreatedAt = DateTime.UtcNow,
//            CreatedBy = "Me",
//            Article = "BABY-0001",
//            Description = "Подгузники размер 4, 54 шт",
//            IsDeleted = false,
//            Name = "Подгузники",
//            Quantity = 11,
//            Price = 1399,
//            Category = new Category
//            {
//                Id = Guid.Parse("30000000-0000-0000-0000-000000000024"),
//                Name = "Детские товары",
//                Description = "Детские товары"
//            },
//            Manufacturer = new Manufacturer
//            {
//                Id = Guid.Parse("40000000-0000-0000-0000-000000000024"),
//                Name = "Pampers",
//                Country = "Польша"
//            },
//            SectionId = BabySectionId
//        }
//    };
//}