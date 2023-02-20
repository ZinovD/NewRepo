using FurnitureStore.Data.Models;
using Microsoft.AspNetCore.Hosting;
using System.Collections.Generic;
using System.Linq;

namespace FurnitureStore.Data
{
    public class DbObjects
    {
        private readonly IWebHostEnvironment _environment;
        public DbObjects(IWebHostEnvironment environment)
        {
            _environment = environment;
        }
        public static void Initial(ApplicationContext context)
        {
            if (!context.FurnitureKind.Any())
                context.FurnitureKind.AddRange(Kinds.Select(c => c.Value));
            if (!context.HomePage.Any())
                context.HomePage.AddRange(Pages.Select(c => c.Value));
            if (!context.FurnitureType.Any())
                context.FurnitureType.AddRange(Types.Select(c => c.Value));
            if (!context.FurnitureColor.Any())
                context.FurnitureColor.AddRange(Colors.Select(c => c.Value));
            if (!context.Stock.Any())
                context.Stock.AddRange(Stocks.Select(c => c.Value));
            context.SaveChanges();
        }
        private static Dictionary<string, FurnitureKind> kinds;
        private static Dictionary<string, FurnitureKind> Kinds
        {
            get
            {
                if (kinds == null)
                {
                    var list = new FurnitureKind[]
                    {
                        new FurnitureKind
                        {
                            Kind="Кухонная", Photo="/img/KindPhoto/1.jpg",
                        },
                        new FurnitureKind
                        {
                            Kind="Для спальни", Photo="/img/KindPhoto/2.jpg"
                        },
                        new FurnitureKind
                        {
                            Kind="Для гостиной", Photo="/img/KindPhoto/3.jpg"
                        },
                        new FurnitureKind
                        {
                            Kind="Для ресторанов", Photo="/img/KindPhoto/4.jpg"
                        },
                        new FurnitureKind
                        {
                            Kind="Для офисов", Photo="/img/KindPhoto/5.jpg"
                        },
                        new FurnitureKind
                        {
                            Kind="Дизайнерская", Photo="/img/KindPhoto/6.jpg"
                        }
                    };
                    kinds = new Dictionary<string, FurnitureKind>();
                    foreach(FurnitureKind el in list)
                        kinds.Add(el.Kind, el); 
                }
                return kinds;
            }
        }

        private static Dictionary<string, FurnitureType> types;
        private static Dictionary<string, FurnitureType> Types
        {
            get
            {
                if (types == null)
                {
                    var list = new FurnitureType[]
                    {
                        new FurnitureType
                        {
                            Type="Диваны",
                        },
                        new FurnitureType
                        {
                            Type="Кресла",
                        },
                        new FurnitureType
                        {
                            Type="Стулья",
                        },
                        new FurnitureType
                        {
                            Type="Кровати",
                        },
                        new FurnitureType
                        {
                            Type="Шкафы",
                        }
                    };
                    types = new Dictionary<string, FurnitureType>();
                    foreach (FurnitureType el in list)
                        types.Add(el.Type, el);
                }
                return types;
            }
        }
        private static Dictionary<string, FurnitureColor> colors;
        private static Dictionary<string, FurnitureColor> Colors
        {
            get
            {
                if (colors == null)
                {
                    var list = new FurnitureColor[]
                    {
                        new FurnitureColor
                        {
                            Color="#000000",
                            ColorName="Чёрный"
                        },
                        new FurnitureColor
                        {
                            Color="#f60000",
                            ColorName="Красный"
                        },
                        new FurnitureColor
                        {
                            Color="#ffff00",
                            ColorName="Жёлтый"
                        },
                        new FurnitureColor
                        {
                            Color="#0000ff",
                            ColorName="Синий"
                        },
                        new FurnitureColor
                        {
                            Color="#fe00ff",
                            ColorName="Розовый"
                        }
                    };
                    colors = new Dictionary<string, FurnitureColor>();
                    foreach (FurnitureColor el in list)
                        colors.Add(el.Color, el);
                }
                return colors;
            }
        }
        static Dictionary<string, Stock> stocks;
        private static Dictionary<string, Stock> Stocks
        {
            get
            {
                if (stocks == null)
                {
                    var list = new Stock[]
                    {
                        new Stock
                        {
                            StockName="Нет",
                        }
                    };
                    stocks = new Dictionary<string, Stock>();
                    foreach (Stock el in list)
                        stocks.Add(el.StockName, el);
                }
                return stocks;
            }
        }

        private static Dictionary<string, HomePage> pages;
        private static Dictionary<string, HomePage> Pages
        {
            get
            {
                if (pages == null)
                {
                    Stock stock=new Stock() {StockName="Нет"};
                    var list = new HomePage[]
                    {

                        new HomePage
                        {
                            Name = "Главная страница",
                            ImageBlock = true,
                            CharacteristicsBlock = false,
                            StockBlock = false,
                            KindBlock = true,
                            OurChoiceBlock = true,
                            TitleImage_1 = "/img/TitleImage/1_1.jpg",
                            TitleImage_2 = "/img/TitleImage/1_2.jpg",
                            TitleImage_3 = "/img/TitleImage/1_3.jpg",
                            CircleImage_1 = "/img/CircleImage/1_1.jpg",
                            CircleImage_2 = "/img/CircleImage/1_2.jpg",
                            CircleImage_3 = "/img/CircleImage/1_3.jpg",
                            CircleText_1 = "Экологичные материалы",
                            CircleText_2 = "Быстрая доставка",
                            CircleText_3 = "Гарантия 2 года",
                        },
                    };
                    pages = new Dictionary<string, HomePage>();
                    foreach (HomePage el in list)
                    pages.Add(el.Name, el);
                }
                return pages;
        }
    }
}   
}
