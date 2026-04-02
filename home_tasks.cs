using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;

namespace HomeworkATM
{
    internal class home_tasks
    {
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
            public Dictionary<int, int> Caset { get; set; }
            public List<string> History { get; set; }
            public string Secret_word { get => "pasword"; }

            public ATM(string bank, Dictionary<int, int> caset)
            {
                Random rnd = new Random();
                ID_ATM = ((long)rnd.Next() << 32) | (uint)rnd.Next();
                Bank = bank;
                Caset = caset;
                History = new List<string>();
            }
            public override string ToString()
            {
                if (this.History.Count > 0)
                {
                    return $"Банкомат - {this.ID_ATM},банк - {this.Bank},номиналы купюр и их количество:\n{String.Join("\n", this.Caset.Select(x => $"{x.Key} {x.Value}"))} \n История транзакций:\n{String.Join("\n", this.History)}";
                }
                else
                {
                    return $"Банкомат - {this.ID_ATM},банк - {this.Bank},номиналы купюр и их количество:\n{String.Join("\n", this.Caset.Select(x => $"{x.Key} {x.Value}"))} \n";
                }
            }
            public int CashAmount()
            {
                int count = 0;
                foreach (int val in this.Caset.Values)
                {
                    count += val;

                }
                return count;
            }
            public void replenishment_card(Card card, Dictionary<int, int> data)
            {
                int[] possible_nominals = new int[] { 50, 100, 200, 500, 1000, 2000, 5000 };
                int sum_replenishment = 0;
                foreach (int key in data.Keys)
                {
                    if (!possible_nominals.Contains(key))
                    {
                        History.Add($"{card.ToString()}: Пополнение (сумма неизвестна из-за неправильного номинала купюр) => Отклонено");
                        throw new ArgumentException("купюры такого номенала не существует, введите другое значение");

                    }
                    sum_replenishment += key * data[key];
                    if (Caset.ContainsKey(key))
                    {
                        Caset[key] += 1;
                    }
                    else
                    {
                        Caset[key] = 1;
                    }


                }
                string[] posible_month = new string[] { "01", "02", "03", "04", "05", "06", "07", "08", "09", "10", "11", "12" };
                string current_data = "04/2026";
                if (int.Parse(card.month_year.Split('/')[1]) < int.Parse(current_data.Split('/')[1]) || (Array.IndexOf(posible_month, card.month_year.Split('/')[0]) < Array.IndexOf(posible_month, current_data.Split('/')[0]) && (int.Parse(card.month_year.Split('/')[1]) == int.Parse(current_data.Split('/')[1]))))
                {
                    History.Add($"{card.ToString()}: Пополнение ({sum_replenishment}) => Отклонено,истек срок годности");
                    throw new ArgumentException("карта больше не действительна истек ее срок годности");
                }
                if (card.Bank != Bank)
                {
                    card.Balance += (int)Math.Round(sum_replenishment * 0.95);
                    Console.WriteLine("Так как ваша карта от другого банка, с вас будет удержана комисия 5%");
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
                    card.Balance += (int)Math.Round(withdrawal_val * 1.05);
                    History.Add($"{card.ToString()}: Снятие ({withdrawal_val}) => Успешно,удержана комисия 5%");
                }
                else
                {
                    card.Balance += withdrawal_val;
                    History.Add($"{card.ToString()}: Снятие ({withdrawal_val}) => Успешно");
                }

            }
            public void Pickup(string key)
            {
                List<char> alfavit = new List<char>();
                for (int i = 97; i <= 122; i++)
                {
                    alfavit.Add((char)i);
                }
                string new_word = "";
                foreach (char c in key)
                {
                    if (alfavit.IndexOf(char.ToLower(c)) < 4 )
                    {
                        new_word += (alfavit[(alfavit.Count - alfavit.IndexOf(char.ToLower(c)))]);
                    }
                    else 
                    {
                        new_word += (alfavit[alfavit.IndexOf(char.ToLower(c)) - 4]);
                    }
                    
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
            ATM atm = new ATM("Сбербанк", new Dictionary<int, int> { { 500, 1 }, { 1000, 5 }, { 5000, 3 }, { 100, 22 }, { 200, 10 }, { 2000, 12 } });
            Console.WriteLine(atm.ToString());
            Card card = new Card("2200123445676869", "Maksim", "10/2024", "Т-Банк", 500);
            Card card1 = new Card("2200123445676869", "Maksim", "10/2027", "Т-Банк", 500);
            Card card2 = new Card("2200123445676869", "Maksim", "10/2024", "Т-Банк", 500);
            Console.WriteLine(card.ToString());
            Console.WriteLine(atm.CashAmount());
            atm.replenishment_card(card, new Dictionary<int, int> { { 500, 2 }, { 1000, 1 }, { 2000, 2 } });
            //atm.replenishment_card(card, new Dictionary<int, int> { { 500, 2 }, { 1200, 1 }, { 2000, 2 } });
            //atm.replenishment_card(card1, new Dictionary<int, int> { { 500, 2 }, { 1000, 1 }, { 2000, 2 } });
            //atm.replenishment_card(card2, new Dictionary<int, int> { { 500, 2 }, { 1000, 1 }, { 2000, 2 } });
            atm.withdrawal_card(card, 500);
            //atm.withdrawal_card(card, 5000);
            //atm.withdrawal_card(card1, 500);
            //atm.withdrawal_card(card2, 500);
        }
    }
}
