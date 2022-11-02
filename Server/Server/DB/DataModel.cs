using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Server.DB
{
	[Table("Account")]
	public class AccountDb
	{
		public int AccountDbId { get; set; }
		public string AccountName { get; set; }
		public ICollection<PlayerDb> Players { get; set; }
	}

	[Table("Player")]
	public class PlayerDb
	{
		public int PlayerDbId { get; set; }
		public string PlayerName { get; set; }

		[ForeignKey("Account")]
		public int AccountDbId { get; set; }
		public AccountDb Account { get; set; }

		public ICollection<ItemDb> Items { get; set; }
		public ICollection<QuestDb> Quests { get; set; }
	}

	[Table("Item")]
	public class ItemDb
	{
		public int ItemDbId { get; set; }
		public int TemplateId { get; set; }

		[ForeignKey("Owner")]
		public int? OwnerDbId { get; set; }
		public PlayerDb Owner { get; set; }
	}

	[Table("Quest")]
	public class QuestDb
	{
		public int QuestDbId { get; set; }
		public int TemplateId { get; set; }
		public bool IsCleared { get; set; }

		[ForeignKey("Owner")]
		public int? OwnerDbId { get; set; }
		public PlayerDb Owner { get; set; }
	}

	[Table("Region")]
	public class RegionDb
    {
		public int RegionDbId { get; set; }
		public string RegionName { get; set; }
		public ICollection<FestivalDb> Festivals { get; set; }
    }

	[Table("Festival")]
	public class FestivalDb
    {
		public int FestivalDbId { get; set; }
		public int TemplateId { get; set; }
		public string FestivalName { get; set; }
		public string FestivalExplain { get; set; }
		public string RelatedAddress { get; set; }

		[ForeignKey("Region")]
		public int RegionDbId { get; set; }
		public RegionDb Region { get; set; }
	}
}