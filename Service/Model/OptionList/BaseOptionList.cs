﻿using Service.Exceptions;
using Service.Model.Option;

namespace Service.Model.OptionList
{
    public abstract class BaseOptionList<TOption> where TOption : BaseOption
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string? Description { get; set; }

        public string? SelectedOption { get; set; }

        protected readonly List<TOption> _options = [];
        public IReadOnlyList<TOption> Options => _options.AsReadOnly();

        public BaseOptionList(string name)
        {
            Name = name ?? throw new EmptyListNameException(GetType().Name);
        }
    }
}
