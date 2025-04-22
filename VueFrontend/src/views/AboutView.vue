<template>
  <form class="w-full min-h-screen mx-auto p-6 bg-white dark:bg-gray-900 space-y-6">
    <!-- İkili grid input alanları -->
    <div class="grid grid-cols-1 md:grid-cols-2 gap-6">
      <div>
        <label for="first_name" class="block text-sm font-medium text-gray-700 dark:text-gray-200 mb-1"
          >First Name</label
        >
        <input
          type="text"
          id="first_name"
          placeholder="John"
          class="w-full p-3 border border-gray-300 rounded-lg text-sm text-gray-900 dark:text-white bg-gray-50 dark:bg-gray-800 focus:ring-2 focus:ring-blue-500 focus:border-blue-500"
          required
        />
      </div>
      <div>
        <label for="last_name" class="block text-sm font-medium text-gray-700 dark:text-gray-200 mb-1">Last Name</label>
        <input
          type="text"
          id="last_name"
          placeholder="Doe"
          class="w-full p-3 border border-gray-300 rounded-lg text-sm text-gray-900 dark:text-white bg-gray-50 dark:bg-gray-800 focus:ring-2 focus:ring-blue-500 focus:border-blue-500"
          required
        />
      </div>
      <div>
        <label for="company" class="block text-sm font-medium text-gray-700 dark:text-gray-200 mb-1">Company</label>
        <input
          type="text"
          id="company"
          placeholder="Your Company"
          class="w-full p-3 border border-gray-300 rounded-lg text-sm text-gray-900 dark:text-white bg-gray-50 dark:bg-gray-800 focus:ring-2 focus:ring-blue-500 focus:border-blue-500"
          required
        />
      </div>
      <div>
        <label for="phone" class="block text-sm font-medium text-gray-700 dark:text-gray-200 mb-1">Phone</label>
        <input
          type="tel"
          id="phone"
          placeholder="123-45-678"
          class="w-full p-3 border border-gray-300 rounded-lg text-sm text-gray-900 dark:text-white bg-gray-50 dark:bg-gray-800 focus:ring-2 focus:ring-blue-500 focus:border-blue-500"
          pattern="[0-9]{3}-[0-9]{2}-[0-9]{3}"
          required
        />
      </div>
      <div>
        <label for="website" class="block text-sm font-medium text-gray-700 dark:text-gray-200 mb-1">Website</label>
        <input
          type="url"
          id="website"
          placeholder="example.com"
          class="w-full p-3 border border-gray-300 rounded-lg text-sm text-gray-900 dark:text-white bg-gray-50 dark:bg-gray-800 focus:ring-2 focus:ring-blue-500 focus:border-blue-500"
          required
        />
      </div>
      <div>
        <label for="visitors" class="block text-sm font-medium text-gray-700 dark:text-gray-200 mb-1"
          >Monthly Visitors</label
        >
        <input
          type="number"
          id="visitors"
          class="w-full p-3 border border-gray-300 rounded-lg text-sm text-gray-900 dark:text-white bg-gray-50 dark:bg-gray-800 focus:ring-2 focus:ring-blue-500 focus:border-blue-500"
          required
        />
      </div>
    </div>

    <!-- Email -->
    <div>
      <label for="email" class="block text-sm font-medium text-gray-700 dark:text-gray-200 mb-1">Email</label>
      <input
        type="email"
        id="email"
        placeholder="john.doe@example.com"
        class="w-full p-3 border border-gray-300 rounded-lg text-sm text-gray-900 dark:text-white bg-gray-50 dark:bg-gray-800 focus:ring-2 focus:ring-blue-500 focus:border-blue-500"
        required
      />
    </div>

    <!-- Şifre -->
    <div class="grid grid-cols-1 md:grid-cols-2 gap-6">
      <div>
        <label for="password" class="block text-sm font-medium text-gray-700 dark:text-gray-200 mb-1">Password</label>
        <input
          type="password"
          id="password"
          placeholder="••••••••"
          class="w-full p-3 border border-gray-300 rounded-lg text-sm text-gray-900 dark:text-white bg-gray-50 dark:bg-gray-800 focus:ring-2 focus:ring-blue-500 focus:border-blue-500"
          required
        />
      </div>
      <div>
        <label for="confirm_password" class="block text-sm font-medium text-gray-700 dark:text-gray-200 mb-1"
          >Confirm Password</label
        >
        <input
          type="password"
          id="confirm_password"
          placeholder="••••••••"
          class="w-full p-3 border border-gray-300 rounded-lg text-sm text-gray-900 dark:text-white bg-gray-50 dark:bg-gray-800 focus:ring-2 focus:ring-blue-500 focus:border-blue-500"
          required
        />
      </div>
    </div>

    <!-- Checkbox -->
    <div class="flex items-start">
      <input
        type="checkbox"
        id="terms"
        class="mt-1 w-4 h-4 text-blue-600 bg-gray-100 border-gray-300 rounded focus:ring-blue-500 dark:bg-gray-800 dark:border-gray-600"
      />
      <label for="terms" class="ml-2 text-sm text-gray-700 dark:text-gray-300"
        >I agree to the
        <a href="#" class="text-blue-600 hover:underline dark:text-blue-400">terms and conditions</a>.</label
      >
    </div>

    <!-- Submit -->
    <div>
      <button
        type="submit"
        class="w-full md:w-auto bg-blue-600 hover:bg-blue-700 text-white px-6 py-2.5 rounded-lg text-sm font-medium focus:outline-none focus:ring-4 focus:ring-blue-300 dark:bg-blue-500 dark:hover:bg-blue-600 dark:focus:ring-blue-700"
      >
        Submit
      </button>
    </div>
  </form>
</template>

<script setup></script>
