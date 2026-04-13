using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Linq.Expressions;
using System.Runtime.ExceptionServices;
using System.Text;
using System.Threading.Tasks;

namespace HometaskATM
{
    internal class task_atm_2
    {
        public class Banknote
        {
            public int num_banknote { get; }
            public string banknote_series { get; }
            public Banknote(int num_banknote, string banknote_series)
            {
                this.num_banknote = num_banknote;
                this.banknote_series = banknote_series;
            }
            public override string ToString()
            {
                return $"Номинал - {num_banknote}, Серия - {banknote_series}";
            }
        }
        public class Card
        {
            private int _balance;
            public string number_card { get; }
            public string name_owner { get; }
            public string month_year { get; }
            public string Bank { get; }
            public int Balance
            {
                get => _balance;
                set
                {
                    if (value < 0)
                    {
                        throw new ArgumentException("Баланс не может быть отрицательным, введите положительный баланс");
                    }
                    else
                    {
                        _balance = value;
                    }
                }
            }
            public Card(string Number_card, string name_Owner, string Month_year, string bank, int balance)
            {
                if (Number_card.Length != 16)
                {
                    throw new ArgumentException("Длина номера карты не может быть меньше или больше 16, ввеедите допустимое значение");
                }
                else
                {
                    number_card = Number_card;
                }
                string[] posible_month = new string[] { "01", "02", "03", "04", "09", "05", "06", "07", "08", "10", "11", "12" };

                name_owner = name_Owner;
                if (Month_year.Contains('/') && posible_month.Contains(Month_year.Split('/')[0]))
                {
                    if (int.TryParse(Month_year.Split('/')[1], out int result))
                    {
                        if (result > 0)
                        {
                            month_year = Month_year;
                        }
                        else
                        {
                            throw new ArgumentException("год не может быть отрицательным или равным нулю, введите допустимое значение");
                        }
                    }
                    else
                    {
                        throw new ArgumentException("год не может быть переведен в число, введите допустимое значение");
                    }

                }
                else
                {
                    throw new ArgumentException("Неверно введен месяц");
                }
                Bank = bank;
                Balance = balance;
            }
            public override string ToString()
            {
                return $"Карта: {this.Bank} {this.number_card} {this.month_year}, владелец {name_owner}, баланс: {this.Balance} ";
            }
        }
        public class ATM
        {
            public long ID_ATM { get; }
            public string Bank { get; }
            public Dictionary<int, Stack<Banknote>> Caset { get; set; }
            public List<string> History { get; set; }
            public string Secret_word { get => "12351545783"; }

            public ATM(string bank, Dictionary<int, Stack<Banknote>> caset)
            {
                Random rnd = new Random();
                ID_ATM = ((long)rnd.Next() << 32) | (uint)rnd.Next();
                Bank = bank;
                Caset = caset;
                History = new List<string>();
            }
            public override string ToString()
            {
                if (this.History.Count > 0 && this.Caset.Count != 0)
                {
                    return $"Банкомат - {this.ID_ATM},банк - {this.Bank},номиналы купюр и их количество:\n{String.Join("\n", this.Caset.Select(x => $"{x.Key} {x.Value.Select(y => y.ToString())}"))} \n История транзакций:\n{String.Join("\n", this.History)}";
                }
                if (this.History.Count == 0 && this.Caset.Count != 0)
                {
                    return $"Банкомат - {this.ID_ATM},банк - {this.Bank},номиналы купюр и их количество:\n{String.Join("\n", this.Caset.Select(x => $"{x.Key} {x.Value.Select(y => y.ToString())}"))} \n";
                }
                if (this.Caset.Count == 0 && this.History.Count != 0)
                {
                    return $"Банкомат - {this.ID_ATM},банк - {this.Bank},Купюр в бакомате нет\n История транзакций:\n{String.Join("\n", this.History)}";
                }
                else
                {
                    return $"Банкомат - {this.ID_ATM},банк - {this.Bank},Купюр в бакомате нет\n";
                }
            }
            public int CashAmount()
            {
                int count = 0;
                foreach (Stack<Banknote> val in this.Caset.Values)
                {
                    count += val.Select(x => x.num_banknote).Sum();
                }
                return count;
            }
            public void replenishment_card(Card card, Dictionary<int, Stack<Banknote>> data)
            {
                int[] possible_nominals = new int[] { 50, 100, 200, 500, 1000, 2000, 5000 };
                int sum_replenishment = 0;
                string[] posible_month = new string[] { "01", "02", "03", "04", "05", "06", "07", "08", "09", "10", "11", "12" };
                string current_data = "04/2026";
                if (int.Parse(card.month_year.Split('/')[1]) < int.Parse(current_data.Split('/')[1]) || (Array.IndexOf(posible_month, card.month_year.Split('/')[0]) < Array.IndexOf(posible_month, current_data.Split('/')[0]) && (int.Parse(card.month_year.Split('/')[1]) == int.Parse(current_data.Split('/')[1]))))
                {
                    History.Add($"{card.ToString()}: Пополнение ({sum_replenishment}) => Отклонено,истек срок годности");
                    throw new ArgumentException("карта больше не действительна истек ее срок годности");
                }
                foreach (int key in data.Keys)
                {
                    if (!possible_nominals.Contains(key))
                    {
                        History.Add($"{card.ToString()}: Пополнение (сумма неизвестна из-за неправильного номинала купюр) => Отклонено");
                        throw new ArgumentException("купюры такого номенала не существует, введите другое значение");

                    }

                    foreach (string item_serias in data[key].Select(x => x.banknote_series))
                    {
                        int sum_series_numbers = 0;
                        if (!(Math.Abs((int)char.ToLower(item_serias[0]) - (int)char.ToLower(item_serias[1])) + 1 % 2 == 0))
                        {
                            throw new ArgumentException("у вашей купюры неправильный серийный номер");
                        }
                        foreach (char num in item_serias.Skip(2).Take(9))
                        {
                            sum_series_numbers += num - '0';
                        }
                        if (sum_series_numbers % 2 == 0)
                        {
                            throw new ArgumentException("у вашей купюры неправильный серийный номер");
                        }
                    }
                    sum_replenishment += key * data[key].Select(x => x.num_banknote).Sum();
                    if (Caset.ContainsKey(key))
                    {
                        foreach (Banknote item in data[key])
                        {
                            Caset[key].Push(item);
                        }
                    }
                    else
                    {
                        Caset[key] = data[key];
                    }


                }

                if (card.Bank != Bank)
                {
                    card.Balance += (int)Math.Round(sum_replenishment * 0.95);
                    Console.WriteLine("Так как ваша карта от другого банка, с вас будет удержана комисия 5%");
                    Console.WriteLine($"Были приняты следующие купюры \n{String.Join("\n", data.Values.Select(x => $"{x.ToString()}"))}");
                    History.Add($"{card.ToString()}: Пополнение ({sum_replenishment}) => Успешно,удержана комисия 5%");
                }
                else
                {
                    card.Balance += sum_replenishment;
                    History.Add($"{card.ToString()}: Пополнение ({sum_replenishment}) => Успешно");
                }
            }
            public void withdrawal_card(Card card, int withdrawal_val)
            {
                string[] posible_month = new string[] { "01", "02", "03", "04", "05", "06", "07", "08", "09", "10", "11", "12" };
                string current_data = "04/2026";
                if (int.Parse(card.month_year.Split('/')[1]) < int.Parse(current_data.Split('/')[1]) || (Array.IndexOf(posible_month, card.month_year.Split('/')[0]) < Array.IndexOf(posible_month, current_data.Split('/')[0]) && (int.Parse(card.month_year.Split('/')[1]) == int.Parse(current_data.Split('/')[1]))))
                {
                    History.Add($"{card.ToString()}: Снятие ({withdrawal_val}) => Отклонено,истек срок годности карты");
                    throw new ArgumentException("карта больше не действительна истек ее срок годности");
                }
                if (card.Balance < withdrawal_val)
                {
                    History.Add($"{card.ToString()}: Снятие ({withdrawal_val}) => Отклонено, на балансе недостаточно средств");
                    throw new ArgumentException("На балансе недостаточно средст для снятия.");
                }
                if (Bank != card.Bank)
                {
                    Console.WriteLine("Так как ваша карта от другого банка, с вас будет удержана комисия 5%");
                    card.Balance -= (int)Math.Round(withdrawal_val * 1.05);
                    History.Add($"{card.ToString()}: Снятие ({withdrawal_val}) => Успешно,удержана комисия 5%");
                }
                else
                {
                    card.Balance -= withdrawal_val;
                    History.Add($"{card.ToString()}: Снятие ({withdrawal_val}) => Успешно");
                }

            }
            public void Pickup(string key)
            {
                if (!int.TryParse(key, out int result) ||  key.Length < 3)
                {
                    throw new ArgumentException("код должен содержать только цифры и не менее 3, полиция уже выехала");
                }
                string new_word = "";
                string previous_three = key.Take(3).ToString();
                int sum_of_three = 0;
                foreach (char c in previous_three) 
                {
                    sum_of_three += (c - '0');
                }
                new_word += sum_of_three.ToString()[2];
                for (int i = 0; i < key.Length - 3; i++) 
                {
                    sum_of_three = 0;
                    string current_three = previous_three.Skip(1).Take(2).ToString() + (key[i + 3]); 
                    previous_three = current_three;
                    foreach (char c in current_three) 
                    {
                        sum_of_three += (c - '0');
                    }
                    new_word += sum_of_three.ToString()[2];
                }
                if (new_word != Secret_word)
                {

                    throw new ArgumentException("Полиция уже выехала никуда не уходите");
                }
                Caset.Clear();
            }
        }

        static void Main()
        {
            ATM atm = new ATM("Сбербанк", new Dictionary<int, Stack<Banknote>> { { 500, new Stack<Banknote>(new Banknote[] { new Banknote(500, "АМ123329922"), new Banknote(500, "АМ123329922") }) }, { 1000, new Stack<Banknote>(new Banknote[] { new Banknote(1000, "АМ127729922"), new Banknote(500, "АМ127329922") }) }, { 5000, 3 }, { 100, 22 }, { 200, 10 }, { 2000, 12 } });
            Console.WriteLine(atm.ToString());
            Card card = new Card("2200123445676869", "Maksim", "10/2028", "Т-Банк", 500);
            Card card1 = new Card("2200123445676869", "Maksim", "10/2022", "Т-Банк", 500);
            Card card2 = new Card("2200123445676869", "Maksim", "03/2026", "Т-Банк", 500);
            Card card3 = new Card("2200123445676869", "Maksim", "07/2026", "Сбербанк", 500);
            Console.WriteLine(card.ToString());
            Console.WriteLine(atm.CashAmount());
            atm.replenishment_card(card, new Dictionary<int, int> { { 500, 2 }, { 1000, 1 }, { 2000, 2 } });
            //atm.replenishment_card(card, new Dictionary<int, int> { { 500, 2 }, { 1200, 1 }, { 2000, 2 } });
            //atm.replenishment_card(card1, new Dictionary<int, int> { { 500, 2 }, { 1000, 1 }, { 2000, 2 } });
            //atm.replenishment_card(card2, new Dictionary<int, int> { { 500, 2 }, { 1000, 1 }, { 2000, 2 } });
            atm.replenishment_card(card3, new Dictionary<int, int> { { 500, 2 }, { 1000, 1 }, { 2000, 2 } });
            Console.WriteLine(atm.ToString());
            Console.WriteLine(card.Balance);
            atm.withdrawal_card(card, 500);
            //atm.withdrawal_card(card, 10000);
            //atm.withdrawal_card(card1, 500);
            //atm.withdrawal_card(card2, 500);
            atm.withdrawal_card(card3, 6000);
            Console.WriteLine(atm.ToString());
            Console.WriteLine(card.Balance);
            atm.Pickup("tewwasvh");
            Console.WriteLine(atm.ToString());
            atm.Pickup("tewwasvs");
        }
    }
}
