using AutoMapper.Configuration.Conventions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Transactocommerce.Controllers.Queiries
{
    public class ProductQuery
    {
        private const int _defaultPage = 0;
        private int _page = _defaultPage;

        private const int _maxSize = 100;
        private const int _defaultSize = 10;

        private int _size = _defaultSize;

        public int Page {
            get
            {
                return _page;
            }
            set
            {
                if (value < 0)
                    _page = _defaultPage;
                else
                    _page = value;
            }
        }

        public int Size
        {
            get
            {
                return _size;
            }
            set
            {
                if (value < 0)
                    _size = _defaultSize;
                else
                    _size = Math.Min(_maxSize, value);
            }
        }
    }
}
