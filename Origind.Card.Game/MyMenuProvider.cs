﻿using System.Collections.Generic;
using Newbe.Mahua;

namespace Origind.Card.Game
{
    public class MyMenuProvider : IMahuaMenuProvider
    {
        public IEnumerable<MahuaMenu> GetMenus()
        {
            return new[]
            {
                new MahuaMenu
                {
                    Id = "menu1",
                    Text = "测试菜单1"
                },
                new MahuaMenu
                {
                    Id = "menu2",
                    Text = "测试菜单2"
                },
            };
        }
    }
}
