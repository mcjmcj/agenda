using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace Agenda.Models
{
    public class Telefone
    {
        public long Codigo { get; set; }
        public string Numero { get; set; }
        public string Tipo { get; set; }

        public enum Tipos
        {
            Casa,
            Trabalho,
            Outro
        }

        public IEnumerable<SelectListItem> TelefoneTipoSelectListItems
        {
            get
            {
                foreach (Tipos tipo in Enum.GetValues(typeof(Tipos)))
                {
                    SelectListItem selectListItem = new SelectListItem();
                    selectListItem.Text = tipo.ToString();
                    selectListItem.Value = tipo.ToString();
                    selectListItem.Selected = Tipo == tipo.ToString();

                    yield return selectListItem;
                }
            }
        }

    }    
}