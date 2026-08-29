import random

def calculate_fitness(genotype):
    
    total_pairs = n * (n - 1) // 2  
    attacking_pairs = 0

    for i in range(n):
        for j in range(i + 1, n):
            if abs(j - i) == abs(genotype[j] - genotype[i]):
                attacking_pairs += 1

    return total_pairs - attacking_pairs


def generate_population(size=100):
    """
 
    """
    population = []
    base = list(range(1, 9))
    for _ in range(size):
        g = base[:]       
        random.shuffle(g)   
        population.append(g)
    return population




perfect_solution = [4, 2, 7, 3, 6, 8, 5, 1]
fitness = calculate_fitness(perfect_solution)
print(f"Mükemmel bir çözümün uygunluğu: {fitness}")  # Beklenen: 28


initial_population = generate_population(100)
print(f"{len(initial_population)} genotipten oluşan bir popülasyon oluşturuldu.")
print("Popülasyondaki ilk genotip:", initial_population[0])
print("İlk genotipin uygunluğu:", calculate_fitness(initial_population[0]))
